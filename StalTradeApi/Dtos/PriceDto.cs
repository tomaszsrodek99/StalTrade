using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StalTradeAPI.Dtos
{
    public class PriceDto
    {
        public int PriceId { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public ProductDto? Product { get; set; }
        [Display(Name = "Firma"), Required(ErrorMessage = "Ceny dla wszystkich dostępnych firm są już ustalone.")]
        public int CompanyId { get; set; }
        public CompanyDto? Company { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Netto")]
        [DataType(DataType.Currency)]
        public decimal Netto { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Zakup?")]
        public bool IsPurchase { get; set; }
    }
}
