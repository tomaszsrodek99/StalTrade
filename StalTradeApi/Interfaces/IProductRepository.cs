using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllProductWithPricesAsync();
        bool IsProductExists(string companyDrawingNumber, int productId);
    }
}
