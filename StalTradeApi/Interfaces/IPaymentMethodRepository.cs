using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IPaymentMethodRepository : IGenericRepository<PaymentMethod>
    {
        bool MethodExists(string request);
    }
}
