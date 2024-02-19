using StalTradeAPI.Dtos;
using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IPriceRepository : IGenericRepository<Price>
    {
        Task<IEnumerable<Price>> GetAllPrices();
    }
}
