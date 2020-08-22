using System;
using Expensely.Domain.Transactions;

namespace Expensely.Tests.Common.Entities
{
    public static class TransactionData
    {
        public static readonly string Name = "Bills";

        public static readonly int InvalidCurrencyId = default;

        public static readonly Currency Currency = Currency.Usd;

        public static readonly decimal MinusOneAmount = -1.0m;

        public static readonly Money MinusOne = new Money(MinusOneAmount, Currency);

        public static readonly decimal PlusOneAmount = 1.0m;

        public static readonly Money PlusOne = new Money(PlusOneAmount, Currency);

        public static Expense CreateExpense(Guid? userId = null, DateTime? date = null)
            => new Expense(Guid.NewGuid(), userId ?? Guid.NewGuid(), Name, MinusOne, date ?? Time.Now());

        public static Income CreateIncome()
            => new Income(Guid.NewGuid(), Guid.NewGuid(), Name, PlusOne, Time.Now());
    }
}
