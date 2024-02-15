using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class DepositDto
    {
        public int Id
        {
            get; set;
        }
        [Display(Name = "Kwota")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal Cash
        {
            get; set;
        }
        [Required, Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}
