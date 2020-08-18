using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;

namespace Expensely.Tests.Common.Entities
{
    public static class ExpenseData
    {
        public static readonly string Name = "Bills";

        public static readonly int InvalidCurrencyId = 0;

        public static readonly Currency Currency = Currency.FromId(1) ?? throw new ArgumentException("currencyId");

        public static readonly decimal MinusOneAmount = -1.0m;

        public static readonly Money MinusOne = new Money(MinusOneAmount, Currency);

        public static Expense CreateExpense(Guid? userId = null, DateTime? date = null)
            => new Expense(Guid.NewGuid(), userId ?? Guid.NewGuid(), Name, MinusOne, date ?? Time.Now());
    }
}
