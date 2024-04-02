using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        private readonly StalTradeDbContext _context;
        public CompanyRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Company>> GetAllCompaniesWithContactsAsync()
        {
            return await _context.Companies.Include(c => c.Contacts).ToListAsync();
        }
        public bool IsNIPExists(string nip, int companyId)
        {
            return _context.Companies.Any(c => c.NIP == nip && c.CompanyID != companyId);
        }
    }
}
