using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class InvoiceProductRepository : GenericRepository<InvoiceProduct>, IInvoiceProductRepository
    {
        private readonly StalTradeDbContext _context;
        public InvoiceProductRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
