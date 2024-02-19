using StalTradeAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StalTradeAPI.Dtos
{
    public class StockStatusDto
    {
        public int StockStatusId { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Produkt")]
        public int ProductId { get; set; }

        public ProductDto Product { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Ilośc kupiona")]
        public int PurchasedQuantity { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Ilość rzeczywista")]
        public int ActualQuantity { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Ilośc sprzedana")]
        public int SoldQuantity { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Ilość magazynowa")]
        public int InStock { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Kwota Zakupu")]
        [DataType(DataType.Currency)]
        public decimal PurchasedValue { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Kwota sprzedaży")]
        [DataType(DataType.Currency)]
        public decimal SoldValue { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Wartość marży")]
        [DataType(DataType.Currency)]
        public decimal MarginValue { get; set; }
        [Required(ErrorMessage = "To pole jest wymagane."), Display(Name = "Marża")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal Margin { get; set; }
    }
}
