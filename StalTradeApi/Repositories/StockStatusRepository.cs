using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class StockStatusRepository : GenericRepository<StockStatus>, IStockStatusRepository
    {
        private readonly StalTradeDbContext _context;
        public StockStatusRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<StockStatus>> GetAllProductsAsync()
        {
            var products = await _context.StockStatuses
                .Include(ss => ss.Product)
                .ToListAsync();

            return products;
        }
    }
}
