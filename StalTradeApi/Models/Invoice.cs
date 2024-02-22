using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [Required, MaxLength(64), MinLength(2)]
        public string InvoiceNumber { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Netto { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Brutto { get; set; }

        [Required]
        public bool IsPurchase { get; set; }
        [Required]
        public IEnumerable<InvoiceProduct> ProductsList { get; set; }
    }
}
