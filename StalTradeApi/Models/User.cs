using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string Firstname { get; set; } = string.Empty;
    }
}
