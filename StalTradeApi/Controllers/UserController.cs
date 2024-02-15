using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        public UserController(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _repository = userRepository;
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _repository.GetAllAsync();
                if (users == null)
                {
                    return BadRequest("Nie znaleziono użytkowników");
                }
                var records = _mapper.Map<List<UserDto>>(users);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUser{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _repository.GetAsync(id);
                if (user == null)
                {
                    return BadRequest("Nie znaleziono użytkownika.");
                }
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> PutUser(UserDto userDto)
        {
            try
            {
                var user = await _repository.GetAsync(userDto.Id);
                if (user == null)
                {
                    return BadRequest("Nie znaleziono użytkownika o podanym ID.");
                }
                var users = await _repository.GetAllAsync();
                var duplicate = users.Where(x => x.Email == userDto.Email && x.Id != userDto.Id);
                if (duplicate.Any())
                {
                    return BadRequest("Użytkownik o podanym adresie email już istnieje.");
                }
                _mapper.Map(userDto, user);

                await _repository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("Delete{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _repository.GetAsync(id);
                if (user == null)
                {
                    return NotFound("Nie znaleziono użytkownika o podanym adresie email.");
                }
                await _repository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
