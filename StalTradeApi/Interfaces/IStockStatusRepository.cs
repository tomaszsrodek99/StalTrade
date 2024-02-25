using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IStockStatusRepository : IGenericRepository<StockStatus>
    {
        Task<IEnumerable<Product>> GetAllProductsWithStockStatusAsync();
        Task<StockStatus> GetAsyncByProductId(int id);
    }
}
