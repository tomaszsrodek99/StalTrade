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
            return _context.Products.Any(c => c.CompanyDrawingNumber == companyDrawingNumber && c.ProductId != productId);
        }

        public async Task<IEnumerable<Product>> GetAllProductWithPrices()
        {
            var products = await _context.Products
            .Include(p => p.Prices)
                .ThenInclude(price => price.Company)
            .Select(product => new Product
            {
                ProductId = product.ProductId,
                UnitOfMeasure = product.UnitOfMeasure,
                Name = product.Name,
                CompanyDrawingNumber = product.CompanyDrawingNumber,
                Prices = product.Prices.Select(price => new Price
                {
                    PriceId = price.PriceId,
                    ProductId = price.ProductId,
                    CompanyId = price.CompanyId,
                    Date = price.Date,
                    Netto = price.Netto,
                    IsPurchase = price.IsPurchase,
                    Company = new Company
                    {
                        Name = price.Company.Name
                    },
                }).ToList()
            })
            .ToListAsync();

            return products;
        }

        public async Task<IEnumerable<Product>> GetAllProductsWithLatestPrices()
        {
            var products = await GetAllProductWithPrices();

            return products;
        }
        public async Task<Product> GetAsyncWithStockStatus(int productId)
        {
            var product = await _context.Products
                .Include(s => s.StockStatus)
                .Where(p => p.ProductId == productId)
                .FirstOrDefaultAsync();
            return product;
        }

        public override async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            bool isUsedInInvoices = await _context.Invoices.AnyAsync(i => i.ProductsList.Any(p => p.ProductId == id));

            bool isUsedInPrices = await _context.Prices.AnyAsync(p => p.ProductId == id);

            if (isUsedInInvoices || isUsedInPrices)
            {
                throw new InvalidOperationException("Nie można usunąć tego produktu, ponieważ jest już zapisany w fakturze bądź jego cena jest zapisana w bazie danych.");
            }

            base.DeleteAsync(id);
        }
    }
}
