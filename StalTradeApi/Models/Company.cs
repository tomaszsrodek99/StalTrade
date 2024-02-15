using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }
        [Required, MaxLength(64),MinLength(2)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(64),MinLength(2)]
        public string ShortName { get; set; } = string.Empty;
        [Required, MaxLength(64), MinLength(2)]
        public string Address { get; set; } = string.Empty;
        [Required, MaxLength(64), MinLength(2)]
        public string City { get; set; } = string.Empty;
        [Required,StringLength(6, MinimumLength = 6)]
        public string PostalCode { get; set; } = string.Empty;
        [Required, MaxLength(64), MinLength(2)]
        public string PostOffice { get; set; } = string.Empty;
        [Required,StringLength(10, MinimumLength = 10)]
        public string NIP { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public List<Contact>? Contacts { get; set; }
    }
}
