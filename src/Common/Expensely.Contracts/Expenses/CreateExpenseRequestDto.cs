namespace Expensely.Contracts.Expenses
{
    public class CreateExpenseRequestDto
    {
        public decimal Amount { get; set; }

        public int CurrencyId { get; set; }
    }
}
