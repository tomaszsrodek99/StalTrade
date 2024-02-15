using System.ComponentModel.DataAnnotations;

namespace StalTradeAPI.Dtos
{
    public class MonthlyExpenseViewModel
    {
        public string Month { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal TotalBrutto { get; set; }
        public IEnumerable<ExpenseDto> Expenses { get; set; }
    }
}
