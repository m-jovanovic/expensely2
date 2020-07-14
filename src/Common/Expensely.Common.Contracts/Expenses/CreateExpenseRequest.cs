using System;

namespace Expensely.Common.Contracts.Expenses
{
    public class CreateExpenseRequest
    {
        public CreateExpenseRequest()
        {
            Name = string.Empty;
        }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public int CurrencyId { get; set; }

        public DateTime Date { get; set; }
    }
}
