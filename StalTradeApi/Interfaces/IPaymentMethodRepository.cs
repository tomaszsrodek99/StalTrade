using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod>
    {
        public bool MethodExists(string request);
    }
}
