using AutoMapper;
using Dto;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using RepresentativesTracking;
using Modle.Model;
using System;

namespace Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _CustomerService;
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        public CustomerController(IMapper mapper, ICustomerService CustomerService,ICompanyService companyService)
        {
            _CustomerService = CustomerService;
            _companyService = companyService;
            _mapper = mapper;
        }
        [HttpGet("{Id}", Name = "GetCustomerById")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<CustomerReadDto>> GetCustomerById(Guid Id)
        {
            var result = await _CustomerService.GetById(Id, Guid.Parse(GetClaim("CompanyID")));
            if (result == null)
            {
                return NotFound();
            }
            if (GetClaim("Role") != "Admin" && result.CompanyID.ToString() != GetClaim("CompanyID"))
                return BadRequest(new { Error = "لا يمكن عرض زبائن الشركات الأخرى من دون صلاحية المدير" });
            var CustomerModel = _mapper.Map<CustomerReadDto>(result);
            return Ok(CustomerModel);
        }
        [HttpGet]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<ActionResult<CustomerReadDto>> GetAllCustomers()
        {
            var result = await _CustomerService.GetAll();
            var CustomerModel = _mapper.Map<IList<CustomerReadDto>>(result);
            return Ok(CustomerModel);
        }
        [HttpGet("{PageNumber}/{Count}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<ActionResult<CustomerReadDto>> GetCustomersByCompany(int PageNumber,int Count)
        {
            var result = await _CustomerService.GetCustomersByCompany(Guid.Parse(GetClaim("CompanyID")),PageNumber,Count);
            var CustomerModel = _mapper.Map<IList<CustomerReadDto>>(result);
            return Ok(CustomerModel);
        }
        [HttpPost]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerWriteDto CustomerWriteDto)
        {
            var CustomerModel = _mapper.Map<Customer>(CustomerWriteDto);
            CustomerModel.Company =await _companyService.FindById(Guid.Parse(GetClaim("CompanyID")));
            await _CustomerService.Create(CustomerModel);
            var CustomerReadDto = _mapper.Map<CustomerReadDto>(CustomerModel);
            return CreatedAtRoute("GetCustomerById", new { Id = CustomerReadDto.Id }, CustomerReadDto);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> UpdateCustomer(Guid Id, [FromBody] CustomerWriteDto CustomerWriteDto)
        {
            var CustomerModelFromRepo = await _CustomerService.FindById(Id);
            if (CustomerModelFromRepo == null)
            {
                return NotFound();
            }
            if (GetClaim("Role") != "Admin" && CustomerModelFromRepo.CompanyID.ToString() != GetClaim("CompanyID"))
                return BadRequest(new { Error = "لا يمكن تعديل زبائن الشركات الأخرى من دون صلاحية المدير" });
            var CustomerModel = _mapper.Map<Customer>(CustomerWriteDto);
            await _CustomerService.Modify(Id, CustomerModel);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> DeleteCustomer(Guid Id)
        {
            var Customer = await _CustomerService.Delete(Id);
            if (Customer == null)
            {
                return NotFound();
            }
            if (GetClaim("Role") != "Admin" && Customer.CompanyID.ToString() != GetClaim("CompanyID"))
                return BadRequest(new { Error = "لا يمكن حذف زبائن الشركات الأخرى من دون صلاحية المدير" });
            return NoContent();
        }
    }
}