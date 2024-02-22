using StalTradeAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StalTradeAPI.Dtos
{
    public class InvoiceProductDto
    {
        [Key]
        public int InvoiceProductId { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Numer faktury")]
        public int InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        [JsonIgnore]
        public Invoice? Invoice { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Produkt")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Ilość")]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Netto")]
        public decimal Netto { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Brutto")]
        public decimal Brutto { get; set; }
    }
}
