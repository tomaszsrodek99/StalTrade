using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IExpenseRepository : IGenericRepository<Expense>
    {
        List<string> GetContractorsFromDatabase(string term);
        List<string> GetDescriptionsFromDatabase(string term);
    }
}
