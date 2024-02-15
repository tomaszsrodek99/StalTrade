using StalTradeAPI.Models;
using StalTradeAPI.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Dtos;
using StalTradeAPI.Context;

namespace StalTradeAPI.Repositories
{

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly StalTradeDbContext _context;
        public UserRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> UserExists(string request)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == request);
        }

        public async void RegisterFromInitializer(UserRegisterRequestDto request)
        {
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                var newUser = new User()
                {
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };
                await AddAsync(newUser);
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
