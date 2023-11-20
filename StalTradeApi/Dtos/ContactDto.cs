using StalTradeAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StalTradeAPI.Dtos
{
    public class ContactDto
    {
        public int ContactID { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane")]
        public int CompanyID { get; set; }
        [JsonIgnore]
        public Company? Company{ get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name ="Imię"),MaxLength(32, ErrorMessage = "Maksymalna długość dla tego pola wynosi 32 litery."),
            MinLength(2, ErrorMessage = "Minimalna długość dla tego pola wynosi 2 litery."), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery i zawierać tylko litery.")]
        public string Firstname { get; set; } = string.Empty;
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Nazwisko"), MaxLength(32, ErrorMessage = "Maksymalna długość dla tego pola wynosi 32 litery."),
           MinLength(2, ErrorMessage = "Minimalna długość dla tego pola wynosi 2 litery."), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery i zawierać tylko litery.")]
        public string Lastname { get; set; } = string.Empty;
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Stanowisko"), MaxLength(64, ErrorMessage = "Maksymalna długość dla tego pola wynosi 32 litery."),
           MinLength(2, ErrorMessage = "Minimalna długość dla tego pola wynosi 2 litery."), RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłńóśźż0-9 ]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery i zawierać tylko litery.")]
        public string Position { get; set; } = string.Empty;
        [Required(ErrorMessage = "To pole jest wymagane."), StringLength(9, ErrorMessage = "Pole musi być długości 9 cyfr."), Display(Name = "Tel. 1"), RegularExpression("^[0-9]*$", ErrorMessage = "Pole musi zawierać tylko cyfry.")]
        public string Phone1 { get; set; } = string.Empty;
        [StringLength(9, ErrorMessage = "Pole musi być długości 9 cyfr.", MinimumLength = 9)]
        [Display(Name = "Tel. 2")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Pole musi zawierać tylko cyfry.")]
        public string? Phone2 { get; set; }
        [MaxLength(32, ErrorMessage = "Maksymalna długość dla tego pola wynosi 32 litery."), MinLength(8, ErrorMessage = "Minimalna długość to 8 znaków"), EmailAddress(ErrorMessage = "Pole musi być formatu email - xyz@gmail.com.")]
        public string Email { get; set; } = string.Empty;
        public ContactDto(int companyID)
        {
            CompanyID = companyID;
        }
        public ContactDto()
        {
        }
    }
}
