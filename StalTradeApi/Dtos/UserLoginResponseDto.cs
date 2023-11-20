using StalTradeAPI.Models;

namespace StalTradeAPI.Dtos
{
    public class UserLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
