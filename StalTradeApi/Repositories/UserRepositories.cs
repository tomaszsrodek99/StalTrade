using StalTradeAPI.Models;
using StalTradeAPI.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StalTradeAPI.Dtos;
using StalTradeAPI.Context;
using System.ComponentModel;

namespace StalTradeAPI.Repositories
{

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private IConfiguration _config;
        private readonly StalTradeDbContext _context;
        public UserRepository(StalTradeDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _config = configuration;
        }

        public async Task<User> UserExists(string request)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == request);
        }

    }
}
