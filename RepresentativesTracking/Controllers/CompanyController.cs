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
        [HttpGet("{Id}", Name = "GetCompanyById")]
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
        public async Task<ActionResult<CompanyReadDto>> GetAllCompanies()
        {
            var result = await _companyService.GetAll();
            var CompanyModel = _mapper.Map<IList<CompanyReadDto>>(result);
            return Ok(CompanyModel);
        }
        [HttpPost]
        public async Task<IActionResult> AddCompany([FromBody] CompanyWriteDto CompanyWriteDto)
        {
            var CompanyModel = _mapper.Map<Company>(CompanyWriteDto);
            await _companyService.Create(CompanyModel);
            var CompanyReadDto = _mapper.Map<CompanyReadDto>(CompanyModel);
            return CreatedAtRoute("GetCompanyById", new { Id = CompanyReadDto.ID }, CompanyReadDto);
        }
        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
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