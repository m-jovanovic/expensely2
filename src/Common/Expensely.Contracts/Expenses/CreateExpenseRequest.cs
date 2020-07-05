namespace Expensely.Contracts.Expenses
{
    public class CreateExpenseRequest
    {
        public decimal Amount { get; set; }

        public int CurrencyId { get; set; }
    }
}
