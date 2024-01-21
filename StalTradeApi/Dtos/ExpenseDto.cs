using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class ExpenseDto
    {
        public int ExpenseId { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Data")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Dostawca"), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."), MinLength(2, ErrorMessage = "Minimalna długość to 2 litery."),
        RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery.")]
        public string Contractor { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Nr. faktury")]
        [MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."), MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string InvoiceNumber { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Opis"), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery.")]
        [MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."), MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Netto")]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Netto { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Brutto")]
        [Column(TypeName = "decimal(5, 2)")]
        public decimal Brutto { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Termin płatności")]
        public DateTime DateOfPayment { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Zapłacono")]
        public bool Paid { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Płatność"), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery."),
        MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."), MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string PaymentType { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Typ zdarzenia"), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
        MinLength(2, ErrorMessage = "Minimalna długość to 2 litery."),RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery.")]
        public string EventType { get; set; }
    }
}
