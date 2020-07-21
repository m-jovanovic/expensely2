using System;

namespace Expensely.Application.Contracts.Expenses
{
    public sealed class ExpenseDto
    {
        public ExpenseDto()
        {
            Name = string.Empty;
            CurrencyCode = string.Empty;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyCode { get; set; }

        public string FormattedExpense => $"{Amount:n2} {CurrencyCode}";

        public DateTime Date { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime? ModifiedOnUtc { get; set; }

        public bool Deleted { get; set; }
    }
}
