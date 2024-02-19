using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery."), Display(Name = "Nazwa")]
        public string Name { get; set; }
    }
}
