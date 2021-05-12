using AutoMapper;
using Dto;
using Model;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepresentativesTracking;
using Modle.Model;
using System.Linq;

namespace Controllers
{
    [Route("api/[action]")]
    //[Authorize(Roles = UserRole.Admin)]

    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ICompanyService _companyService;
        private readonly IUserService _userService;

        private readonly IMapper _mapper;
        public OrderController(IMapper mapper, IOrderService orderService,IUserService userService,ICompanyService companyService)
        {
            _orderService = orderService;
            _userService = userService;
            _companyService = companyService;
            _mapper = mapper;
        }

        [HttpGet("{Id}", Name = "GetOrderById")]
        public async Task<ActionResult<OrderReadDto>> GetOrderById(int Id)
        {
            var result = await _orderService.FindById(Id);
            var User = await _userService.FindById(result.UserID);
            User.Company = await _companyService.FindById(User.CompanyID.Value);
            var UserReadDto = _mapper.Map<UserReadDto>(User);
            if (result == null)
            {
                return NotFound();
            }
            var OrderModel = _mapper.Map<OrderReadDto>(result);
            OrderModel.User = UserReadDto;
            return Ok(OrderModel);
        }
        [HttpGet]
        public async Task<ActionResult<OrderReadDto>> GetOrderByUser(int PageNumber,int Count)
        {
            var result = _orderService.GetOrderByUser(Convert.ToInt32(GetClaim("ID")),PageNumber,Count).Result.ToList();
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
            }
            return Ok(OrderModel);
        }
        [HttpGet]
        public async Task<ActionResult<OrderReadDto>> GetAllOrders()
        {
            var result = _orderService.GetAll().Result.ToList();
            User User;
            UserReadDto UserReadDto;
            List<OrderReadDto> OrderModel = new List<OrderReadDto>();
            for (int i = 0; i < result.Count; i++)
            {
                User = await _userService.FindById(result[i].UserID);
                User.Company = await _companyService.FindById(User.CompanyID.Value);
                UserReadDto = _mapper.Map<UserReadDto>(User);
                OrderModel = _mapper.Map<List<OrderReadDto>>(result);
                OrderModel[i].User = UserReadDto;
            }
            return Ok(OrderModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderWriteDto OrderWriteDto)
        {
            var OrderModel = _mapper.Map<Order>(OrderWriteDto);
            var Order=await _orderService.Create(OrderModel);
            var User = await _userService.FindById(Order.UserID);
            var UserReadDto = _mapper.Map<UserReadDto>(User);
            var OrderReadDto = _mapper.Map<OrderReadDto>(OrderModel);
            OrderReadDto.User = UserReadDto;
            return CreatedAtRoute("GetOrderById", new { Id = OrderReadDto.ID }, OrderReadDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int Id, [FromBody] OrderWriteDto OrderWriteDto)
        {
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
        public async Task<IActionResult> StartOrder(int Id, [FromBody] OrderStartDto OrderStartDto)
        {
            var Order =await _orderService.FindById(Id);
            if (Order.ISInProgress==false||Order.ReceiptImageUrl!=null)
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
        public async Task<IActionResult> EndOrder(int Id, [FromBody] OrderEndDto OrderEndDto)
        {
            var OrderModelFromRepo = await _orderService.FindById(Id);
            OrderModelFromRepo.User = await _userService.FindById(OrderModelFromRepo.UserID);
            if (OrderModelFromRepo == null)
            {
                return NotFound();
            }
            var OrderModel = _mapper.Map<Order>(OrderEndDto);
            await _orderService.EndModify(Id, OrderModel);
            return NoContent();
        }
        [HttpDelete("{id}")]
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