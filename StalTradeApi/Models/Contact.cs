using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StalTradeAPI.Models
{
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }
        [Required]
        [ForeignKey("Company")]
        public int CompanyID { get; set; }
        [JsonIgnore]
        public Company? Company { get; set; }
        [Required, MaxLength(32), MinLength(2)]
        public string Firstname { get; set; } = string.Empty;
        [Required, MaxLength(32), MinLength(2)]
        public string Lastname { get; set; } = string.Empty;
        [Required, MaxLength(64), MinLength(2)]
        public string Position { get; set; } = string.Empty;
        [Required, StringLength(9)]
        public string Phone1 { get; set; } = string.Empty;
        [StringLength(9)]
        public string? Phone2 { get; set; }
        [MaxLength(32), MinLength(8)]
        public string Email { get; set; } = string.Empty;
    }
}
