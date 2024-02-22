using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StalTradeAPI.Models
{
    public class InvoiceProduct
    {
        [Key]
        public int InvoiceProductId { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        [JsonIgnore]
        public Invoice? Invoice { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Netto { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Brutto { get; set; }
    }
}
