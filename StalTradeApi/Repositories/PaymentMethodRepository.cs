using StalTradeAPI.Context;
using StalTradeAPI.Interfaces;
using StalTradeAPI.Models;

namespace StalTradeAPI.Repositories
{
    public class PaymentMethodRepository : GenericRepository<PaymentMethod>, IPaymentMethodRepository
    {
        private readonly StalTradeDbContext _context;
        public PaymentMethodRepository(StalTradeDbContext context) : base(context)
        {
            _context = context;
        }
        public bool MethodExists(string request)
        {
            return !_context.PaymentMethods.Any(c => c.Name == request);
        }
    }
}
