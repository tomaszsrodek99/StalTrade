using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;
using StalTradeAPI.Repositories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IContactRepository _contactRepository;
        public ContactController(IMapper mapper, IContactRepository contactRepository)
        {
            _mapper = mapper;
            _contactRepository = contactRepository;
        }

        [HttpGet("GetContact{contactId}")]
        public async Task<IActionResult> GetContact(int contactId)
        {
            try
            {
                var dto = await _contactRepository.GetAsync(contactId);
                return Ok(_mapper.Map<ContactDto>(dto));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("CreateContact")]
        public async Task<IActionResult> CreateContact(ContactDto dto)
        {
            try
            {
                await _contactRepository.AddAsync(_mapper.Map<Contact>(dto));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("UpdateContact")]
        public async Task<IActionResult> UpdateContact(ContactDto dto)
        {
            try
            {
                await _contactRepository.UpdateAsync(_mapper.Map<Contact>(dto));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteContact{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                await _contactRepository.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
