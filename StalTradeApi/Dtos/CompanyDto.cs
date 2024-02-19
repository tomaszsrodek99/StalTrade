using StalTradeAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class CompanyDto
    {
        public int CompanyID { get; set; }
        [Display(Name = "Nazwa firmy"), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery."),
         Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Skrót"), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery."),
         Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string ShortName { get; set; } = string.Empty;
        [Display(Name = "Adres"), Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(4, ErrorMessage = "Minimalna długość to 4 litery."), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery.")]
        public string Address { get; set; } = string.Empty;
        [Display(Name = "Miasto"), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż ]+$", ErrorMessage = "Pole musi zaczynać się od dużej litery oraz zawierać tylko litery."),
         Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string City { get; set; } = string.Empty;
        [Display(Name = "Kod pocztowy"), RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy powinien mieć format XX-XXX."),
         Required(ErrorMessage = "Pole Kod pocztowy jest wymagane."), StringLength(6, ErrorMessage = "Długość pola musi wynosić 6 znaków.", MinimumLength = 6)]
        public string PostalCode { get; set; } = string.Empty;
        [Display(Name = "Poczta"), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż ]+$", ErrorMessage = "Pole musi zaczynać się od dużej litery oraz zawierać tylko litery."),
         Required(ErrorMessage = "Pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery.")]
        public string PostOffice { get; set; } = string.Empty;
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "NIP może zawierać tylko cyfry."), Required(ErrorMessage = "Pole jest wymagane."),
         StringLength(10, ErrorMessage = "Długość NIPu musi wynosić 10 cyfr.", MinimumLength = 10)]
        public string NIP { get; set; } = string.Empty;
        [Display(Name = "Forma płatności"), Required(ErrorMessage = "Pole jest wymagane.")]
        public string PaymentMethod { get; set; } = string.Empty;
        public List<ContactDto>? Contacts { get; set; }
    }
}
