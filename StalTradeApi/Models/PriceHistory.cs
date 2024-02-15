using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StalTradeAPI.Models
{
    public class PriceHistory
    {
        [Key]
        public int PriceHistoryID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public bool IsPurchase { get; set; } 
        [Required, Display(Name = "Data")]
        public DateTime EventDate { get; set; }        
    }
}
