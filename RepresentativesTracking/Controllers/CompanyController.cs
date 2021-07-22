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
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        public CompanyController(IMapper mapper, ICompanyService companyService)
        {
            _companyService = companyService;
            _mapper = mapper;
        }
        [HttpGet("{Id}", Name = "GetCompanyById")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<ActionResult<CompanyReadDto>> GetCompanyById(int Id)
        {
            var result = await _companyService.FindById(Id);
            if (result == null)
            {
                return NotFound();
            }
            var CompanyModel = _mapper.Map<CompanyReadDto>(result);
            return Ok(CompanyModel);
        }
        [HttpGet]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<ActionResult<CompanyReadDto>> GetAllCompanies()
        {
            var result = await _companyService.GetAll();
            var CompanyModel = _mapper.Map<IList<CompanyReadDto>>(result);
            return Ok(CompanyModel);
        }
        [HttpPost]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> AddCompany([FromBody] CompanyWriteDto CompanyWriteDto)
        {
            var CompanyModel = _mapper.Map<Company>(CompanyWriteDto);
            await _companyService.Create(CompanyModel);
            var CompanyReadDto = _mapper.Map<CompanyReadDto>(CompanyModel);
            return CreatedAtRoute("GetCompanyById", new { Id = CompanyReadDto.ID }, CompanyReadDto);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> UpdateCompany(int Id, [FromBody] CompanyUpdateDto CompanyUpdateDto)
        {
            var CompanyModelFromRepo = await _companyService.FindById(Id);
            if (CompanyModelFromRepo == null)
            {
                return NotFound();
            }
            var CompanyModel = _mapper.Map<Company>(CompanyUpdateDto);
            await _companyService.Modify(Id, CompanyModel);
            return NoContent();
        }
        [HttpPut]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> UpdateCompanyByDirector([FromBody] CompanyUpdateExchangeDto CompanyUpdateExchangeDto)
        {
            var CompanyModelFromRepo = await _companyService.FindById(Convert.ToInt32(GetClaim("CompanyID")));
            if (CompanyModelFromRepo == null)
            {
                return NotFound();
            }
            var CompanyModel = _mapper.Map<Company>(CompanyUpdateExchangeDto);
            await _companyService.ModifyExchange(Convert.ToInt32(GetClaim("CompanyID")), CompanyModel);
            return NoContent();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> DeleteCompany(int Id)
        {
            var Company = await _companyService.Delete(Id);
            if (Company == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}