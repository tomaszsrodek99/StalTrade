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

        public async Task<bool> UserExists(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return false;
   
            return true;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
