using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        private IConfiguration _config;
        private readonly StalTradeDbContext _context;
        public ContactRepository(StalTradeDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _config = configuration;
        }
    }
}
