using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IStockStatusRepository : IGenericRepository<StockStatus>
    {
        Task<IEnumerable<StockStatus>> GetAllProductsAsync();
    }
}
