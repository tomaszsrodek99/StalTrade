using Microsoft.AspNetCore.Mvc;
using StalTradeApi.Dtos;
using StalTradeApi.Models;

namespace StalTradeApi.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> UserExists(string request);
    }
}
