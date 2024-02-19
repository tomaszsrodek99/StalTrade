using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StalTradeAPI.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required, MaxLength(64), MinLength(2)]
        public string Contractor { get; set; }
        [Required, MaxLength(64), MinLength(2)]
        public string InvoiceNumber { get; set; }
        [Required, MaxLength(64), MinLength(2)]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Netto { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Brutto { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfPayment { get; set; }
        [Required]
        public bool Paid { get; set; }
        [Required, MaxLength(64), MinLength(2)]
        public string PaymentType { get; set; }
        [Required, MaxLength(64), MinLength(2)]
        public string EventType { get; set; }
    }
}
