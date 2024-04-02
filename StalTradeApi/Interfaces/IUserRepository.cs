using StalTradeAPI.Dtos;
using StalTradeAPI.Models;

namespace StalTradeAPI.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> UserExists(string email);
        Task<User> GetUserByEmail(string email);
    }
}
