﻿using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class ExpenseRepository : GenericRepository<Expense>, IExpenseRepository
    {
        private readonly StalTradeDbContext _context;
        public ExpenseRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
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
            var matchingDescriptions = _context.Expenses
                .Where(c => c.Description.Contains(term))
                .Select(c => c.Description)
                .ToList();

            return matchingDescriptions;
        }

        public List<string> GetEventTypesFromDatabase(string term)
        {
            var matchingEventTypes = _context.Expenses
                .Where(c => c.EventType.Contains(term))
                .Select(c => c.EventType)
                .ToList();

            return matchingEventTypes;
        }
    }
}
