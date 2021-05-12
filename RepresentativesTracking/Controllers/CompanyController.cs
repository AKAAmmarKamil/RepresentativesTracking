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
    public class CompanyController : BaseController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        public CompanyController(IMapper mapper, ICompanyService companyService)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        [HttpGet("{Id}", Name = "GetCountryById")]
        public async Task<ActionResult<CompanyWriteDto>> GetCompanyById(int Id)
        {
            var result = await _companyService.FindById(Id);
            if (result == null)
            {
                return NotFound();
            }
            var CountryModel = _mapper.Map<CompanyWriteDto>(result);
            return Ok(CountryModel);
        }
        [HttpGet]
        public async Task<ActionResult<CompanyReadDto>> GetAllCompanies()
        {
            var result = await _companyService.GetAll();
            var CountryModel = _mapper.Map<IList<CompanyReadDto>>(result);
            return Ok(CountryModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] CompanyWriteDto CompanyWriteDto)
        {
            var CompanyModel = _mapper.Map<Company>(CompanyWriteDto);
            await _companyService.Create(CompanyModel);
            var CountryReadDto = _mapper.Map<CompanyReadDto>(CompanyModel);
            return CreatedAtRoute("GetCountryById", new { Id = CountryReadDto.ID }, CountryReadDto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int Id, [FromBody] CompanyWriteDto CompanyWriteDto)
        {
            var CountryModelFromRepo = await _companyService.FindById(Id);
            if (CountryModelFromRepo == null)
            {
                return NotFound();
            }
            var CountryModel = _mapper.Map<Company>(CompanyWriteDto);
            await _companyService.Modify(Id, CountryModel);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int Id)
        {
            var Country = await _companyService.Delete(Id);
            if (Country == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}