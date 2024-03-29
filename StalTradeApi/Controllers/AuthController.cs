﻿using Microsoft.AspNetCore.Mvc;
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
                if (await _repository.UserExists(request.Email) != null)
                    return BadRequest("Użytkownik o podanym adresie email już istnieje.");

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var newUser = new User()
                {
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Firstname = request.Firstname
                };

                await _repository.AddAsync(newUser);
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
                var user = await _repository.UserExists(request.Email);
                if (user == null)
                    return BadRequest(new ValidationError() { State = "Email", Message = "Podany email nie istnieje." });

                if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                    return BadRequest(new ValidationError() { State = "Password", Message = "Niepoprawne hasło." });

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
                //new Claim("UserId", user.Id.ToString()),
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
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var user = await _repository.UserExists(email);
            return user != null;
        }
    }
}
