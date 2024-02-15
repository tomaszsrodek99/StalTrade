using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;      
        public CompanyController(IMapper mapper, ICompanyRepository companyRepository)
        {
            _mapper = mapper;
            _companyRepository = companyRepository;
        }

        [HttpGet("GetCompanies")]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies()
        {
            try
            {
                var companies = await _companyRepository.GetAllCompaniesWithContactsAsync();
                if (!companies.Any())
                {
                    return BadRequest("Nie znaleziono użytkowników.");
                }
                var records = _mapper.Map<List<CompanyDto>>(companies);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCompany/{companyId}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(int companyId)
        {
            try
            {
                var company = await _companyRepository.GetAsync(companyId);
                return Ok(_mapper.Map<CompanyDto>(company));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("IsNIPUnique/{companyId}", Name = "IsNIPUnique")]
        public IActionResult IsNIPUnique(int companyId, string nip) 
        {
            return new JsonResult(_companyRepository.IsNIPExists(nip, companyId));
        }   

        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CompanyDto dto)
        {
            try
            {
                await _companyRepository.AddAsync(_mapper.Map<Company>(dto));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }      

        [HttpPut("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany(CompanyDto dto)
        {
            try
            {           
                await _companyRepository.UpdateAsync(_mapper.Map<Company>(dto));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteCompany{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            try
            {
                await _companyRepository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }     
    }
}
