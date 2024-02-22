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
                .Include(s =>s.StockStatus)
                .ToListAsync();
            
            return products;
        }

        public override async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            // Sprawdź, czy ProductId występuje w tabeli Invoice
            bool isUsedInInvoices = await _context.Invoices.AnyAsync(i => i.ProductsList.Any(p => p.ProductId == id));

            // Sprawdź, czy ProductId występuje w tabeli Price
            bool isUsedInPrices = await _context.Prices.AnyAsync(p => p.ProductId == id);

            if (isUsedInInvoices || isUsedInPrices)
            {
                // Jeśli ProductId występuje w Invoice lub Price, nie pozwól na usunięcie produktu
                throw new InvalidOperationException("Nie można usunąć tego produktu, ponieważ jest już zapisany w fakturze bądź jego cena jest zapisana w bazie danych.");
            }

            base.DeleteAsync(id);
        }
    }
}
