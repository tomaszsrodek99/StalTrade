using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 litery."),
         MinLength(2, ErrorMessage = "Minimalna długość to 2 litery."), Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), MaxLength(32, ErrorMessage = "Maksymalna długość to 32 litery."), Display(Name = "Rysunek")]
        public string CompanyDrawingNumber { get; set; }
        [MaxLength(32, ErrorMessage = "Maksymalna długość to 32 litery."), Display(Name = "Rysunek klienta")]
        public string? CustomerDrawingNumber { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Miara")]
        public string UnitOfMeasure { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "VAT zakupu")]
        public int PurchaseVat { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "VAT sprzedaży")]
        public int SalesVat { get; set; }
        [Display(Name = "Norma zużycia")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ConsumptionStandard { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Waga (kg)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Weight { get; set; }
        [Display(Name = "Profil wsadu")]
        public string? ChargeProfile { get; set; } = string.Empty;
        [Display(Name = "Gatunek materiału")]
        public string? MaterialGrade { get; set; } = string.Empty;
        [Display(Name = "Zamiennik")]
        public string? SubstituteGrade { get; set; } = string.Empty;
        public int? StockStatusId { get; set; }
        public StockStatusDto? StockStatus { get; set; }
        public List<PriceDto>? Prices { get; set; }
    }
}
