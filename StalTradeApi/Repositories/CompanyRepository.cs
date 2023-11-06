using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private IConfiguration _config;
        private readonly StalTradeDbContext _context;
        public CompanyRepository(StalTradeDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _config = configuration;
        }
    }
}
