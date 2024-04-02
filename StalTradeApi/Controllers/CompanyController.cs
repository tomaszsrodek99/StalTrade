using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                    return BadRequest("Nie znaleziono zapisanych firm.");
                }

                var contactMap = companies.SelectMany(c => c.Contacts).ToList();

                var contactDtos = _mapper.Map<List<ContactDto>>(contactMap);

                var companyDtos = _mapper.Map<List<CompanyDto>>(companies);

                foreach (var companyDto in companyDtos)
                {
                    companyDto.Contacts = contactDtos.Where(c => c.CompanyID == companyDto.CompanyID).ToList();
                }

                return Ok(companyDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetCompany{companyId}")]
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

        [HttpPost("IsNIPUnique{companyId}")]
        public IActionResult IsNIPUnique(int companyId, [FromBody] string request) 
        {
            bool companyExists = _companyRepository.IsNIPExists(request, companyId);

            if (companyExists)
            {
                return BadRequest("Taka metoda płatności już istnieje.");
            }
            return Ok();
        }   

        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompany(CompanyDto dto)
        {
            try
            {
                await _companyRepository.AddAsync(_mapper.Map<Company>(dto));
                return Ok(new { success = true, message = "Firma została pomyślnie dodana!" });
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
                return Ok(new { success = true, message = "Poprawnie edytowano wartości!" });
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
                return Ok(new { success = true, message = "Pomyślnie usunięto firmę wraz z kontaktami!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }     
    }
}
