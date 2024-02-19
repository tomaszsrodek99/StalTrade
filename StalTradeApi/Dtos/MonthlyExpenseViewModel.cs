using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class MonthlyExpenseViewModel
    {
        [Display(Name = "Miesiąc")]
        public string Month { get; set; }
        [Display(Name = "Suma")]
        [DataType(DataType.Currency)]
        public decimal TotalBrutto { get; set; }
        public IEnumerable<ExpenseDto> Expenses { get; set; }
    }
}
