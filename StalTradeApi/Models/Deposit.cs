using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Models
{
    public class Deposit
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [DataType(DataType.Currency)]
        public decimal Cash { get; set; }
        
    }
}
