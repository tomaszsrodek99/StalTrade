using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class DepositRepository : GenericRepository<Deposit>, IDepositRepository
    {
        private readonly StalTradeDbContext _context;
        public DepositRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
