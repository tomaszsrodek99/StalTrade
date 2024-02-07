namespace StalTradeAPI.Dtos
{
    public class MonthlyExpenseViewModel
    {
        public string Month { get; set; }
        public decimal TotalBrutto { get; set; }
        public IEnumerable<ExpenseDto> Expenses { get; set; }
    }
}
