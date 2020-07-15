using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Domain.Tests.Entities
{
    public sealed class ExpenseTests
    {
        private static readonly string Name = "Expense";
        private static readonly Money Money = new Money(decimal.Zero, Currency.Usd);


        [Fact]
        public void Should_create_properly()
        {
            var id = Guid.NewGuid();
            DateTime now = DateTime.Now;

            var expense = new Expense(id, Name, Money, now);

            Assert.NotNull(expense);
            Assert.Equal(id, expense.Id);
            Assert.Equal(Name, expense.Name);
            Assert.Equal(Money.Amount, expense.Money.Amount);
            Assert.Equal(now.Date, expense.Date);
        }

        [Fact]
        public void Should_be_equal_when_they_have_same_id()
        {
            var id1 = Guid.NewGuid();
            DateTime now = DateTime.Now;

            var expense1 = new Expense(id1, Name, Money, now);
            var expense2 = new Expense(id1, Name, Money, now);

            Assert.True(expense1.Equals(expense2));
            Assert.True(expense1 == expense2);
            Assert.Equal(expense1, expense2);
        }

        [Fact]
        public void Should_not_be_equal_when_they_have_different_id()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            DateTime now = DateTime.Now;

            var expense1 = new Expense(id1, Name, Money, now);
            var expense2 = new Expense(id2, Name, Money, now);

            Assert.False(expense1.Equals(expense2));
            Assert.False(expense1 == expense2);
            Assert.NotEqual(expense1, expense2);
        }
    }
}
