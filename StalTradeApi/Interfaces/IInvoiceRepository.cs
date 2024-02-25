using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<List<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> GetInvoiceWithProducts(int id);
    }
}
