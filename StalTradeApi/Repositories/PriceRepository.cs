using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class PriceRepository : GenericRepository<Price>, IPriceRepository
    {
        private readonly StalTradeDbContext _context;
        public PriceRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Price>> GetAllPrices()
        {
            var prices = await _context.Prices
                .Include(p => p.Product)
                .Include(p => p.Company)
                .ToListAsync();
         
            return prices;
        }
    }
}
