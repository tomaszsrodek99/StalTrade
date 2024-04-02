using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StalTradeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IUserRepository _repository;

        public AuthController(IConfiguration configuration, IUserRepository userRepository)
        {
            _config = configuration;
            _repository = userRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserRegisterRequestDto request)
        {
            try
            {
                if (await CheckEmail(request.Email))
                    return BadRequest("Użytkownik o podanym adresie email już istnieje.");

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                await _repository.AddAsync(new User()
                {
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Firstname = request.Firstname
                });
                return Ok("Poprawna rejestracja");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserLoginRequest request)
        {
            try
            {
                var user = await _repository.GetUserByEmail(request.Email);

                if (user == null)
                    return Unauthorized(new ValidationError() { State = "Email", Message = "Użytkownik o podanym emailu nie istnieje." });

                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                    return Unauthorized(new ValidationError() { State = "Password", Message = "Niepoprawne hasło." });

                string token = CreateToken(user);

                var responseDto = new UserLoginResponseDto() { Token = token, User = user };

                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }
        private string CreateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Firstname)
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        [HttpGet("CheckEmail")]
        public async Task<bool> CheckEmail(string email)
        {
            return await _repository.UserExists(email);
        }
    }
}
