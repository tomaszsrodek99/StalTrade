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
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}
