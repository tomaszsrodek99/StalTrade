using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StalTradeAPI.Models
{
    public class StockStatus
    {
        [Key]
        public int StockStatusId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        [JsonIgnore]
        public Product? Product { get; set; }

        [Required]
        public int PurchasedQuantity { get; set; }

        [Required]
        public int ActualQuantity { get; set; }

        [Required]
        public int SoldQuantity { get; set; }
        [Required]
        public int InStock { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal PurchasedValue { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal SoldValue { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal MarginValue { get; set; }
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal Margin { get; set; }
    }
}
