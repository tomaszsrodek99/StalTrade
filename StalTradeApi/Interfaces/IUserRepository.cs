using StalTradeAPI.Dtos;
using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> UserExists(string request);
        void RegisterFromInitializer(UserRegisterRequestDto request);
    }
}
