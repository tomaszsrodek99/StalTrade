using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class DepositDto
    {
        public int Id { get; set; }
        [Display(Name = "Kwota")]
        [DataType(DataType.Currency)]
        public decimal Cash { get; set; }
        [Required, Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Date { get; set; }
    }
}
