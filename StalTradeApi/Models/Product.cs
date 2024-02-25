using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string CompanyDrawingNumber { get; set; }
        public string? CustomerDrawingNumber { get; set; }
        [Required]
        public string Name { get; set; }      
        [Required]
        public string UnitOfMeasure { get; set; }
        [Required]
        [Column(TypeName = "decimal(5, 2)")]
        public int PurchaseVat { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
        [Required]
        public int SalesVat { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        public decimal? ConsumptionStandard { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Weight { get; set; }
        public string? ChargeProfile { get; set; } = string.Empty;
        public string? MaterialGrade { get; set; } = string.Empty;
        public string? SubstituteGrade { get; set; } = string.Empty;
        public int StockStatusId {  get; set; }
        public StockStatus? StockStatus { get; set; }
        public List<Price>? Prices { get; set; }
    }
}
