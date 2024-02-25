using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class ExpenseDto
    {
        public int ExpenseId { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Data")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Dostawca"), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
        RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery.")]
        public string Contractor { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Nr. faktury/rachunku")]
        [MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery.")]
        public string InvoiceNumber { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Opis")]
        [MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Netto")]
        [DataType(DataType.Currency)]
        public decimal Netto { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Brutto")]
        [DataType(DataType.Currency)]
        public decimal Brutto { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Termin płatności")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfPayment { get; set; }
        [Display(Name = "Zapłacono?")]
        public bool Paid { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Płatność")]
        public string PaymentType { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), Display(Name = "Typ zdarzenia"), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),]
        public string EventType { get; set; }
    }
}
