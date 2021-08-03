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

    [ApiController]
    public class LocationController : BaseController
    {
        private readonly ILocationService _locationService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public LocationController(IMapper mapper, ILocationService locationService,IOrderService orderService,IUserService userService)
        {
            _locationService = locationService;
            _orderService = orderService;
            _userService = userService;
            _mapper = mapper;
        }
        [HttpGet("{Id}", Name = "GetLocationById")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<LocationReadDto>> GetLocationById(Guid Id)
        {
            var result = await _locationService.FindById(Id);
            if (GetClaim("Role") != "Admin" || (GetClaim("Role") != "DeliveryAdmin" && GetClaim("CompanyID") != result.User.CompanyID.ToString()))
            {
                return BadRequest(new { Error = "لا يمكن تعديل بيانات تخص هذا الطلب من دون صلاحية المدير" });
            }
            if (result == null)
            {
                return NotFound();
            }
            var LocationModel = _mapper.Map<LocationReadDto>(result);
            return Ok(LocationModel);
        }
        [HttpGet("{UserId}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<LocationReadDto>> GetLastOfUser(Guid UserId)
        {
            var result = await _locationService.GetLastOfUser(UserId);
            if (GetClaim("Role") != "Admin" || (GetClaim("Role") != "DeliveryAdmin" && GetClaim("CompanyID") != result.User.CompanyID.ToString()))
            {
                return BadRequest(new { Error = "لا يمكن تعديل بيانات تخص هذا الطلب من دون صلاحية المدير" });
            }
            if (result == null)
            {
                return NotFound();
            }
            var LocationModel = _mapper.Map<LocationReadDto>(result);
            return Ok(LocationModel);
        }
        [HttpGet]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<LocationReadDto>> GetAllByOrder()
        {
            var User = Guid.Parse(GetClaim("ID"));
            var Order = _orderService.GetOrderInProgress(User).Result;
            var result = await _locationService.GetAllByOrder(User,Order.ID);
            var LocationModel = _mapper.Map<IList<LocationReadDto>>(result);
            return Ok(LocationModel);
        }
        [HttpGet("{Start}/{End}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<LocationReadDto>> GetAllBetweenTwoDates(DateTime Start,DateTime End)
        {
            var User = Guid.Parse(GetClaim("ID"));
            var result = await _locationService.GetAllBetweenTwoDates(User, Start,End);
            var LocationModel = _mapper.Map<IList<LocationReadDto>>(result);
            return Ok(LocationModel);
        }
        [HttpPost]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Representative)]
        public async Task<IActionResult> AddLocation([FromBody] LocationWriteDto LocationWriteDto)
        {
            var UserId = Guid.Parse(GetClaim("ID"));
            var User =await _userService.FindById(UserId);
            var Order = _orderService.GetOrderInProgress(UserId).Result;
            var LocationModel = _mapper.Map<RepresentativeLocation>(LocationWriteDto);
            LocationModel.User = User;
            LocationModel.Order = Order;
            await _locationService.Create(LocationModel);
            var LocationReadDto = _mapper.Map<LocationReadDto>(LocationModel);
            return CreatedAtRoute("GetLocationById", new { Id = LocationReadDto.ID }, LocationReadDto);
        }
        [HttpPost]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.Representative)]
        public async Task<IActionResult> AddLocationOffline([FromBody] List<LocationOfflineWriteDto> LocationOfflineWriteDto)
        {
            var UserId = Guid.Parse(GetClaim("ID"));
            var User = await _userService.FindById(UserId);
            var Order = _orderService.GetOrderInProgress(UserId).Result;
            var LocationModel = _mapper.Map<List<RepresentativeLocation>>(LocationOfflineWriteDto);
            var LocationReadDto = new List<LocationReadDto>();
            for (int i = 0; i < LocationModel.Count; i++)
            {
                LocationModel[i].User = User;
                LocationModel[i].Order = Order;
                await _locationService.Create(LocationModel[i]);
                LocationReadDto = _mapper.Map<List<LocationReadDto>>(LocationModel);
            }
            return Ok(LocationReadDto);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> DeleteLocation(Guid Id)
        {
            var Location = await _locationService.Delete(Id);
            if (Location == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}