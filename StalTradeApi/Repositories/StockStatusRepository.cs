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
        public async Task<IEnumerable<Product>> GetAllProductsWithStockStatusAsync()
        {
            var products = await _context.Products
                .Include(ss => ss.StockStatus)
                .ToListAsync();

            return products;
        }

        public async Task<StockStatus> GetAsyncByProductId(int id)
        {
            return await _context.StockStatuses.Where(ss => ss.ProductId == id).SingleOrDefaultAsync();
        }
    }
}
