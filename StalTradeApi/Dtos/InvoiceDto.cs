using StalTradeAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class InvoiceDto
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Data wystawienia faktury")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Data płatności")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Numer faktury")]
        [MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
        MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string InvoiceNumber { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Firma")]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Netto")]
        [DataType(DataType.Currency)]
        public decimal Netto { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Brutto")]
        [DataType(DataType.Currency)]
        public decimal Brutto { get; set; }

        [Required]
        public bool IsPurchase { get; set; }
        [Required]

        public IEnumerable<InvoiceProductDto> ProductsList { get; set; }
    }
}
