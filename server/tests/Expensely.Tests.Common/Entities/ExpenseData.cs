using System;
using Expensely.Domain.Transactions;

namespace Expensely.Tests.Common.Entities
{
    public static class ExpenseData
    {
        public static readonly string Name = "Bills";

        public static readonly string InvalidCurrencyCode = string.Empty;

        public static readonly Currency Currency = Currency.FromCode("USD") ?? throw new ArgumentException("currencyCode");

        public static readonly decimal MinusOneAmount = -1.0m;

        public static readonly Money MinusOne = new Money(MinusOneAmount, Currency);

        public static Expense CreateExpense(Guid? userId = null, DateTime? date = null)
            => new Expense(Guid.NewGuid(), userId ?? Guid.NewGuid(), Name, MinusOne, date ?? Time.Now());
    }
}
