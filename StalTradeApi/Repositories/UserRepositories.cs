using StalTradeApi.Models;
using StalTradeApi.Interfaces;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StalTradeApi.Dtos;
using StalTradeApi.Context;
using System.ComponentModel;

namespace StalTradeApi.Repositories
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
