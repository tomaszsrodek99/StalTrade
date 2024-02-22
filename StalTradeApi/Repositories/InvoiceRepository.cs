using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Context;
using StalTradeAPI.Dtos;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        private readonly StalTradeDbContext _context;
        public InvoiceRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Invoice>> GetAllInvoicesAsync()
        {
            var invoices = _context.Invoices
                .Include(p => p.ProductsList)
                    .ThenInclude(pp => pp.Product)
                .Include(c => c.Company)
            .ToListAsync();        

            return invoices;
        }
    }
}
