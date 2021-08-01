using AutoMapper;
using Dto;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepresentativesTracking;
using Modle.Model;
using System.Linq;
using Modle.Form;
using RepresentativesTracking.Attachment;
namespace Controllers
{
    [Route("api/[action]")]

    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;
        private readonly ICustomerService _customerService;
        private readonly UploadImage _uploadImage;
        private readonly IMapper _mapper;
        public OrderController(IMapper mapper, IOrderService orderService,IUserService userService,ICustomerService customerService,ICompanyService companyService,UploadImage uploadImage)
        {
            _uploadImage=uploadImage;
            _orderService = orderService;
            _userService = userService;
            _customerService = customerService;
            _companyService = companyService;
            _mapper = mapper;
        }
        [HttpGet("{Id}", Name = "GetOrderById")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<OrderReadDto>> GetOrderById(int Id)
        {
            var result = await _orderService.FindById(Id);
            var User = await _userService.FindById(result.UserID);
            if (GetClaim("Role") != "Admin" || (GetClaim("Role") != "DeliveryAdmin" && GetClaim("CompanyID") != User.CompanyID.ToString()) || GetClaim("ID") != User.ID.ToString())
            {
                return BadRequest(new { Error = "لا يمكن تعديل بيانات تخص هذا الطلب من دون صلاحية المدير" });
            }
            var UserReadDto = _mapper.Map<UserReadDto>(User);
            if (result == null)
            {
                return NotFound();
            }
            var OrderModel = _mapper.Map<OrderReadDto>(result);
            OrderModel.User = UserReadDto;
            OrderModel.TotalPriceInIQD =await _orderService.GetOrderTotalInIQD(Id);
            OrderModel.TotalPriceInUSD = await _orderService.GetOrderTotalInUSD(Id);
            return Ok(OrderModel);
        }
        [HttpGet("{Id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<OrderReadDto>> GetImageOrder(int Id)
        {
            var result = await _orderService.FindById(Id);
            var User = await _userService.FindById(result.UserID);
            if (GetClaim("Role") != "Admin" || (GetClaim("Role") != "DeliveryAdmin" && GetClaim("CompanyID") != User.CompanyID.ToString()) || GetClaim("ID") != User.ID.ToString())
            {
                return BadRequest(new { Error = "لا يمكن تعديل بيانات تخص هذا الطلب من دون صلاحية المدير" });
            }
            if (result == null)
            {
                return NotFound();
            }
            return File(await _uploadImage.Download(result.ReceiptImageUrl), "Application/Jpeg", new Guid().ToString());
        }
        [HttpGet("{Id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<OrderReadDto>> GetImageUrl(int Id)
        {
            var result = await _orderService.FindById(Id);
            var User = await _userService.FindById(result.UserID);
            if (GetClaim("Role") != "Admin" || (GetClaim("Role") != "DeliveryAdmin" && GetClaim("CompanyID") != User.CompanyID.ToString()) || GetClaim("ID") != User.ID.ToString())
            {
                return BadRequest(new { Error = "لا يمكن تعديل بيانات تخص هذا الطلب من دون صلاحية المدير" });
            }
            if (result == null)
            {
                return NotFound();
            }
            return Ok(new { URL = _orderService.GetURL(result.ReceiptImageUrl) }) ;
        }
        [HttpGet]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin+","+UserRole.Representative)]
        public async Task<ActionResult<OrderReadDto>> GetOrderByUser([FromBody] OrderInfo OrderInfo)
        {
            var result = _orderService.GetOrderByUser(Convert.ToInt32(GetClaim("ID")), OrderInfo.PageNumber, OrderInfo.Count).Result.ToList();
            result =await _orderService.SortOrders(OrderInfo.Longitude,OrderInfo.Latitude,result);
            User User;
            UserReadDto UserReadDto;
            List<OrderReadDto> OrderModel=new List<OrderReadDto>();
            for (int i = 0; i < result.Count; i++)
            {
                User = await _userService.FindById(result[i].UserID);
                User.Company = await _companyService.FindById(User.CompanyID.Value);
                UserReadDto = _mapper.Map<UserReadDto>(User);
                OrderModel = _mapper.Map<List<OrderReadDto>>(result);
                OrderModel[i].User = UserReadDto;
                OrderModel[i].TotalPriceInIQD = await _orderService.GetOrderTotalInIQD(result[i].ID);
                OrderModel[i].TotalPriceInUSD = await _orderService.GetOrderTotalInUSD(result[i].ID);
            }
            return Ok(OrderModel);
        }
        [HttpGet]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<ActionResult<OrderReadDto>> GetAllOrders()
        {
            var result = _orderService.GetAll().Result.ToList();
            User User;
            UserReadDto UserReadDto;
            List<OrderReadDto> OrderModel = new List<OrderReadDto>();
            for (int i = 0; i < result.Count; i++)
            {
                User = await _userService.FindById(result[i].UserID);
                UserReadDto = _mapper.Map<UserReadDto>(User);
                OrderModel = _mapper.Map<List<OrderReadDto>>(result);
                OrderModel[i].User = UserReadDto;
                OrderModel[i].TotalPriceInIQD = await _orderService.GetOrderTotalInIQD(result[i].ID);
                OrderModel[i].TotalPriceInUSD = await _orderService.GetOrderTotalInUSD(result[i].ID);
            }
            return Ok(OrderModel);
        }
        [HttpGet("{PageNumber}/{Count}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<OrderReadDto>> GetAllInCompany(int PageNumber,int Count)
        {
            var result = _orderService.GetAllInCompany(Convert.ToInt32(GetClaim("CompanyID")),PageNumber,Count).Result.ToList();
            User User;
            UserReadDto UserReadDto;
            List<OrderReadDto> OrderModel = new List<OrderReadDto>();
            for (int i = 0; i < result.Count; i++)
            {
                User = await _userService.FindById(result[i].UserID);
                UserReadDto = _mapper.Map<UserReadDto>(User);
                OrderModel = _mapper.Map<List<OrderReadDto>>(result);
                OrderModel[i].User = UserReadDto;
                OrderModel[i].TotalPriceInIQD = await _orderService.GetOrderTotalInIQD(result[i].ID);
                OrderModel[i].TotalPriceInUSD = await _orderService.GetOrderTotalInUSD(result[i].ID);
            }
            return Ok(OrderModel);
        }
        [HttpGet("{Status}/{PageNumber}/{Count}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<OrderReadDto>> GetAllInCompanyByStatus(int Status,int PageNumber, int Count)
        {
            var result = _orderService.GetAllInCompanyByStatus(Convert.ToInt32(GetClaim("CompanyID")),Status, PageNumber, Count).Result.ToList();
            User User;
            UserReadDto UserReadDto;
            List<OrderReadDto> OrderModel = new List<OrderReadDto>();
            for (int i = 0; i < result.Count; i++)
            {
                User = await _userService.FindById(result[i].UserID);
                UserReadDto = _mapper.Map<UserReadDto>(User);
                OrderModel = _mapper.Map<List<OrderReadDto>>(result);
                OrderModel[i].User = UserReadDto;
                OrderModel[i].TotalPriceInIQD = await _orderService.GetOrderTotalInIQD(result[i].ID);
                OrderModel[i].TotalPriceInUSD = await _orderService.GetOrderTotalInUSD(result[i].ID);
            }
            return Ok(OrderModel);
        }
        [HttpPost]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> AddOrder([FromBody] OrderWriteDto OrderWriteDto)
        {
            var OrderModel = _mapper.Map<Order>(OrderWriteDto);
            var Order=await _orderService.Create(OrderModel);
            var User = await _userService.FindById(Order.UserID);
            if (GetClaim("Role") == "DeliveryAdmin" && GetClaim("CompanyID") != User.CompanyID.ToString())
                return BadRequest(new {Error="لا يمكنك إضافة طلب لمندوب من شركة أخرى" });
            var UserReadDto = _mapper.Map<UserReadDto>(User);
            var OrderReadDto = _mapper.Map<OrderReadDto>(OrderModel);
            OrderReadDto.User = UserReadDto;
            OrderReadDto.TotalPriceInIQD = 0;
            OrderReadDto.TotalPriceInUSD = 0;
            return CreatedAtRoute("GetOrderById", new { Id = OrderReadDto.ID }, OrderReadDto);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> UpdateOrder(int Id, [FromBody] OrderWriteDto OrderWriteDto)
        {
            var User = await _userService.FindById(OrderWriteDto.UserID);
            if (GetClaim("Role") == "DeliveryAdmin" && GetClaim("CompanyID") != User.CompanyID.ToString())
            {
                return BadRequest(new { Error = "لا يمكنك تعديل طلب لمندوب من شركة أخرى" });
            }
            var OrderModelFromRepo = await _orderService.FindById(Id);
            if (OrderModelFromRepo == null)
            {
                return NotFound();
            }
            var OrderModel = _mapper.Map<Order>(OrderWriteDto);
            await _orderService.Modify(Id, OrderModel);
            return NoContent();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Representative)]
        public async Task<IActionResult> StartOrder(int Id, [FromBody] OrderStartDto OrderStartDto)
        {
            var Order =await _orderService.FindById(Id);
            if (GetClaim("Role") == "Representative" && GetClaim("CompanyID") != Order.User.CompanyID.ToString())
            {
                return BadRequest(new { Error = "لا يمكنك إختيار طلب تابع لشركة أخرى" });
            }
            if (Order.Status==0||Order.ReceiptImageUrl!=null)
            {
                return BadRequest(new { Error = "لا يمكن إختيار طلب تم تسليمه مسبقاً" });
            }
            if (await _orderService.IsLastOrderCompleted(Convert.ToInt32(GetClaim("ID")))==false)
            {
                return BadRequest(new { Error = "عفواً عليك إنهاء الطلب الحالي أولاً" });
            }
            var OrderModelFromRepo = await _orderService.FindById(Id);
            OrderModelFromRepo.User =await _userService.FindById(OrderModelFromRepo.UserID);
            if (OrderModelFromRepo == null)
            {
                return NotFound();
            }
            var OrderModel = _mapper.Map<Order>(OrderStartDto);
            await _orderService.StartModify(Id, OrderModel);
            return NoContent();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin  + "," + UserRole.Representative)]
        public async Task<IActionResult> DeliveryOrder(int Id, [FromBody] AttachmentString attachment)
        {
            var OrderModelFromRepo = await _orderService.FindById(Id);
            if (OrderModelFromRepo == null)
            {
                return NotFound();
            }
            if (GetClaim("Role") == "Representative" && GetClaim("CompanyID") != OrderModelFromRepo.User.CompanyID.ToString())
            {
                return BadRequest(new { Error = "لا يمكنك إختيار طلب تابع لشركة أخرى" });
            }
            OrderModelFromRepo.User = await _userService.FindById(OrderModelFromRepo.UserID);
            if (attachment == null || attachment.Body == null || !UploadImage.IsBase64(attachment.Body))
            {
                return StatusCode(400, "المرفق غير صالح");
            }
            if (attachment.Body.Length * 3 / 4 > 250_000 * 4)
            {
                return StatusCode(413, " KB250 المرفق غير صالح أو حجم الصورة أكبر من");
            }
            var attachmentId = await _uploadImage.Upload(attachment.Body);
            var OrderEndDto = new OrderDeliveryDto
            {
                ReceiptImageUrl = attachmentId
            };
            var OrderModel = _mapper.Map<Order>(OrderEndDto);
            await _orderService.DeliveryModify(Id, OrderModel);
            return NoContent();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Representative)]
        public async Task<IActionResult> EndOrder(int Id,[FromBody] OrderEndDto OrderEndDto)
        {
            var OrderModelFromRepo = await _orderService.FindById(Id);
            if (OrderModelFromRepo == null)
            {
                return NotFound();
            }
            if (GetClaim("Role") == "Representative" && GetClaim("CompanyID") != OrderModelFromRepo.User.CompanyID.ToString())
            {
                return BadRequest(new { Error = "لا يمكنك إختيار طلب تابع لشركة أخرى" });
            }
            OrderModelFromRepo.User = await _userService.FindById(OrderModelFromRepo.UserID);
            OrderModelFromRepo.Customer = await _customerService.FindById(OrderModelFromRepo.CustomerID);
            OrderModelFromRepo.Status = OrderEndDto.Status;
            await _orderService.EndModify(Id, OrderModelFromRepo);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> DeleteOrder(int Id)
        {
            var Order = await _orderService.Delete(Id);
            if (Order == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}