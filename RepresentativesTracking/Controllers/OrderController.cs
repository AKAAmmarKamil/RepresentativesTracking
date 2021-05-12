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

namespace Controllers
{
    [Route("api/[action]")]
    [Authorize(Roles = UserRole.Admin)]

    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IMapper mapper, IOrderService orderService)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet("{Id}", Name = "GetCountryById")]
        public async Task<ActionResult<OrderWriteDto>> GetOrderById(int Id)
        {
            var result = await _orderService.FindById(Id);
            if (result == null)
            {
                return NotFound();
            }
            var CountryModel = _mapper.Map<OrderWriteDto>(result);
            return Ok(CountryModel);
        }
        [HttpGet]
        public async Task<ActionResult<OrderReadDto>> GetAllOrders()
        {
            var result = await _orderService.GetAll();
            var CountryModel = _mapper.Map<IList<OrderReadDto>>(result);
            return Ok(CountryModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderWriteDto OrderWriteDto)
        {
            var OrderModel = _mapper.Map<Order>(OrderWriteDto);
            await _orderService.Create(OrderModel);
            var CountryReadDto = _mapper.Map<OrderReadDto>(OrderModel);
            return CreatedAtRoute("GetCountryById", new { Id = CountryReadDto.ID }, CountryReadDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int Id, [FromBody] OrderWriteDto OrderWriteDto)
        {
            var CountryModelFromRepo = await _orderService.FindById(Id);
            if (CountryModelFromRepo == null)
            {
                return NotFound();
            }
            var CountryModel = _mapper.Map<Order>(OrderWriteDto);
            await _orderService.Modify(Id, CountryModel);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int Id)
        {
            var Country = await _orderService.Delete(Id);
            if (Country == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}