using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Domain.Tests
{
    public sealed class ExpenseTests
    {
        private static readonly Money Money = new Money(decimal.Zero, Currency.Usd);

        [Fact]
        public void Should_CreateProperly()
        {
            var id = Guid.NewGuid();

            var expense = new Expense(id, Money);

            Assert.NotNull(expense);
            Assert.Equal(id, expense.Id);
            Assert.Equal(Money.Amount, expense.Money.Amount);
        }

        [Fact]
        public void Should_BeEqual_GivenSameId()
        {
            var id1 = Guid.NewGuid();

            var expense1 = new Expense(id1, Money);
            var expense2 = new Expense(id1, Money);

            Assert.True(expense1.Equals(expense2));
            Assert.True(expense1 == expense2);
            Assert.Equal(expense1, expense2);
        }

        [Fact]
        public void Should_NotBeEqual_GivenDifferentIds()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var expense1 = new Expense(id1, Money);
            var expense2 = new Expense(id2, Money);

            Assert.False(expense1.Equals(expense2));
            Assert.False(expense1 == expense2);
            Assert.NotEqual(expense1, expense2);
        }
    }
}
