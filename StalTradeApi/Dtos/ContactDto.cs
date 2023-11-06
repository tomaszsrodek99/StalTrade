using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class ContactDto
    {
        public int ContactID { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane")]
        public int CompanyID { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), MaxLength(32, ErrorMessage = "Maksymalna długość dla tego pola wynosi 32 litery."),
            MinLength(2, ErrorMessage = "Minimalna długość dla tego pola wynosi 2 litery."), RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery i zawierać tylko litery.")]
        public string Firstname { get; set; } = string.Empty;
        [Required(ErrorMessage = "To pole jest wymagane."), MaxLength(32, ErrorMessage = "Maksymalna długość dla tego pola wynosi 32 litery."),
           MinLength(2, ErrorMessage = "Minimalna długość dla tego pola wynosi 2 litery."), RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery i zawierać tylko litery.")]
        public string Lastname { get; set; } = string.Empty;
        [Required(ErrorMessage = "To pole jest wymagane."), MaxLength(32, ErrorMessage = "Maksymalna długość dla tego pola wynosi 32 litery."),
           MinLength(2, ErrorMessage = "Minimalna długość dla tego pola wynosi 2 litery."), RegularExpression("^[A-Z][a-zA-Z]*$", ErrorMessage = "Pole musi zaczynać się od dużej litery i zawierać tylko litery.")]
        public string Position { get; set; } = string.Empty;
        [Required(ErrorMessage = "To pole jest wymagane."), StringLength(9, ErrorMessage = "Pole musi być długości 9 cyfr."), RegularExpression("^[0-9]*$", ErrorMessage = "Pole musi zawierać tylko cyfry.")]
        public string Phone1 { get; set; } = string.Empty;
        [StringLength(9, ErrorMessage = "Pole musi być długości 9 cyfr.", MinimumLength = 9), RegularExpression("^[0-9]*$", ErrorMessage = "Pole musi zawierać tylko cyfry.")]
        public string Phone2 { get; set; } = string.Empty;
        [MaxLength(32, ErrorMessage = "Maksymalna długość dla tego pola wynosi 32 litery."), MinLength(8, ErrorMessage = "Minimalna długość to 8 znaków")]
        public string Email { get; set; } = string.Empty;
    }
}
