using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        private IConfiguration _config;
        private readonly StalTradeDbContext _context;
        public ExpenseRepository(StalTradeDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _config = configuration;
        }

        public List<string> GetContractorsFromDatabase(string term)
        {
            var matchingContractors = _context.Expenses
                .Where(c => c.Contractor.Contains(term))
                .Select(c => c.Contractor)
                .ToList();

            return matchingContractors;
        }

        public List<string> GetDescriptionsFromDatabase(string term)
        {
            var matchingContractors = _context.Expenses
                .Where(c => c.Contractor.Contains(term))
                .Select(c => c.Description)
                .ToList();

            return matchingContractors;
        }
    }
}
