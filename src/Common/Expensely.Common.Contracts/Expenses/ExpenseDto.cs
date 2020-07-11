using System;

namespace Expensely.Common.Contracts.Expenses
{
    public sealed class ExpenseDto
    {
        public ExpenseDto()
        {
            CurrencyCode = string.Empty;
        }

        public Guid Id { get; set; }

        public decimal Amount { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyCode { get; set; }

        public string FormattedExpense => $"{Amount:n2} {CurrencyCode}";

        public DateTime CreatedOnUtc { get; set; }

        public DateTime? ModifiedOnUtc { get; set; }

        public bool Deleted { get; set; }
    }
}
