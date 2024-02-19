using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly StalTradeDbContext _context;
        public ProductRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public bool IsProductExists(string companyDrawingNumber, int productId)
        {
            return !_context.Products.Any(c => c.CompanyDrawingNumber == companyDrawingNumber && c.ProductId != productId);
        }

        public async Task<IEnumerable<Product>> GetAllProductWithPrices()
        {
            var products = await _context.Products             
                .Include(p => p.Prices)
                .ThenInclude(price => price.Company)
                .ToListAsync();

            return products;
        }
    }
}
