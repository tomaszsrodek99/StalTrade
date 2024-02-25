using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        bool IsProductExists(string companyDrawingNumber, int productId);
        Task<IEnumerable<Product>> GetAllProductWithPrices();
        Task<Product> GetAsyncWithStockStatus(int productId);
    }
}
