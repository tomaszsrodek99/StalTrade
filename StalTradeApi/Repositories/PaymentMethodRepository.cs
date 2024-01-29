using Microsoft.EntityFrameworkCore;
using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;
using System.ComponentModel.Design;

namespace StalTradeAPI.Repositories
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod>, IPaymentMethodRepository
    {
        private IConfiguration _config;
        private readonly StalTradeDbContext _context;
        public PaymentMethodRepository(StalTradeDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _config = configuration;
        }
        public bool MethodExists(string request)
        {
            return !_context.PaymentMethods.Any(c => c.Name == request);
        }
    }
}
