using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;

namespace Expensely.Tests.Common.Entities
{
    public static class ExpenseData
    {
        public static readonly string Name = "Bills";

        public static readonly string EmptyName = string.Empty;

        public static readonly int InvalidCurrencyId = 0;

        public static readonly Currency Currency = Currency.FromId(1) ?? throw new ArgumentException("currencyId");
        
        public static readonly decimal ZeroAmount = decimal.Zero;
        
        public static readonly Money Zero = new Money(ZeroAmount, Currency);

        public static Expense Expense => new Expense(Guid.NewGuid(), Name, Zero, Time.Now());
    }
}
