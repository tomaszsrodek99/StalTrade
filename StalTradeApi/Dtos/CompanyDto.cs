using StalTradeAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class CompanyDto
    {
        public int CompanyID { get; set; }
        [Display(Name = "Nazwa firmy"), RegularExpression(@"^[A-Z][A-Za-z0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery oraz zawierać tylko litery."),
         Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Skrót"), RegularExpression(@"^[A-Z][A-Za-z0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery oraz zawierać tylko litery."),
         Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string ShortName { get; set; } = string.Empty;
        [Display(Name = "Adres"), Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(4, ErrorMessage = "Minimalna długość to 4 litery.")]
        public string Address { get; set; } = string.Empty;
        [Display(Name = "Miasto"), RegularExpression(@"^[A-Z][A-Za-z0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery oraz zawierać tylko litery."),
         Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string City { get; set; } = string.Empty;
        [Display(Name = "Kod pocztowy"), RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy powinien mieć format XX-XXX."),
         Required(ErrorMessage = "Pole Kod pocztowy jest wymagane."), StringLength(6, ErrorMessage = "Długość pola musi wynosić 6 znaków.", MinimumLength = 6)]
        public string PostalCode { get; set; } = string.Empty;
        [Display(Name = "Poczta"), RegularExpression(@"^[A-Z][A-Za-z0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery oraz zawierać tylko litery."),
         Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string PostOffice { get; set; } = string.Empty;
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "NIP może zawierać tylko cyfry."), Required(ErrorMessage = "Pole jest wymagane."),
         StringLength(10, ErrorMessage = "Długość NIPu musi wynosić 10 cyfr.", MinimumLength = 10)]
        public string NIP { get; set; } = string.Empty;
        [Display(Name = "Forma płatności")]
        public PaymentMethod PaymentMethod { get; set; }
        public List<Contact>? Contacts { get; set; }
    }
    public enum PaymentMethod
    {
        [Display(Name = "Przelew - 30 dni")]
        BankTransfer30Days,

        [Display(Name = "Przelew - 60 dni")]
        BankTransfer60Days,

        [Display(Name = "Przedpłata")]
        AdvancePayment,

        [Display(Name = "Kompensata")]
        Compensation,

        [Display(Name = "Gotówka")]
        Cash
    }
}
