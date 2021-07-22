using AutoMapper;
using Dto;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepresentativesTracking;
using Modle.Model;
namespace Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly IProductsService _ProductsService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public ProductsController(IMapper mapper, IProductsService ProductsService,IUserService userService,IOrderService orderService)
        {
            _ProductsService = ProductsService;
            _userService = userService;
            _orderService = orderService;
            _mapper = mapper;
        }
        [HttpGet("{Id}", Name = "GetProductsById")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin + "," + UserRole.Representative)]
        public async Task<ActionResult<ProductsReadDto>> GetProductsById(int Id)
        {
            var result = await _ProductsService.FindById(Id);
            if (result == null)
            {
                return NotFound();
            }
            var ProductsModel = _mapper.Map<ProductsReadDto>(result);
            ProductsModel.Order.TotalPriceInIQD = await _orderService.GetOrderTotalInIQD(ProductsModel.Order.ID);
            ProductsModel.Order.TotalPriceInUSD = await _orderService.GetOrderTotalInUSD(ProductsModel.Order.ID);
            return Ok(ProductsModel);
        }
        [HttpGet]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<ActionResult<ProductsReadDto>> GetAllProducts(int PageNumber, int Count)
        {
            var result = await _ProductsService.GetAll(PageNumber, Count);
            var User = new User();
            var ProductsModel = _mapper.Map<IList<ProductsReadDto>>(result);
            for (int i = 0; i < result.Count; i++)
            {
                User = await _userService.FindById(result[i].Order.User.ID);
                result[i].Order.User = User;
                ProductsModel[i] = _mapper.Map<ProductsReadDto>(result[i]);
                ProductsModel[i].Order.TotalPriceInIQD =await _orderService.GetOrderTotalInIQD(ProductsModel[i].Order.ID);
                ProductsModel[i].Order.TotalPriceInUSD = await _orderService.GetOrderTotalInUSD(ProductsModel[i].Order.ID);
            }
            return Ok(ProductsModel);
        }
        [HttpGet]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin + "," + UserRole.Representative)]
        public async Task<ActionResult<ProductsByOrderReadDto>> GetAllProductsByOrder(int OrderId, int PageNumber, int Count)
        {
            var result =await _ProductsService.GetAllByOrder(OrderId, PageNumber, Count);
            var ProductsModel = _mapper.Map<IList<ProductsByOrderReadDto>>(result);
            return Ok(ProductsModel);
        }
        [HttpPost]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> AddProduct([FromBody] ProductsWriteDto ProductsWriteDto)
        {
            var ProductsModel = _mapper.Map<Products>(ProductsWriteDto);
            var Product=await _ProductsService.Create(ProductsModel);
            Product =await _ProductsService.FindById(Product.Id);
            var ProductsReadDto = _mapper.Map<ProductsReadDto>(ProductsModel);
            var User=await _userService.FindById(Product.Order.User.ID);
            ProductsReadDto.Order.User = _mapper.Map<UserReadDto>(User);
            ProductsReadDto.Order.TotalPriceInIQD = await _orderService.GetOrderTotalInIQD(ProductsModel.Order.ID);
            ProductsReadDto.Order.TotalPriceInUSD = await _orderService.GetOrderTotalInUSD(ProductsModel.Order.ID);
            return CreatedAtRoute("GetProductsById", new { Id = ProductsReadDto.Id }, ProductsReadDto);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> UpdateProduct(int Id, [FromBody] ProductsWriteDto ProductsWriteDto)
        {
            var ProductsModelFromRepo = await _ProductsService.FindById(Id);
            if (GetClaim("Role") != "Admin" && ProductsModelFromRepo.Order.User.CompanyID.ToString() != GetClaim("CompanyID"))
                return BadRequest(new { Error = "لا يمكن تعديل منتج يخص شركة أخرى" });
            if (ProductsModelFromRepo == null)
            {
                return NotFound();
            }
            var ProductsModel = _mapper.Map<Products>(ProductsWriteDto);
            await _ProductsService.Modify(Id, ProductsModel);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var Product = await _ProductsService.FindById(Id);
            if (GetClaim("Role") != "Admin" && Product.Order.User.CompanyID.ToString() != GetClaim("CompanyID"))
                return BadRequest(new { Error = "لا يمكن حذف منتج يخص شركة أخرى" });
            await _ProductsService.Delete(Id);
            if (Product == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}