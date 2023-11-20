using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<IEnumerable<Company>> GetAllCompaniesWithContactsAsync();
        bool IsNIPExists(string nip, int companyId);
    }
}
