using System;
using Expensely.Domain.Entities;
using Xunit;

namespace Expensely.Domain.Tests
{
    public sealed class ExpenseTests
    {
        [Fact]
        public void Should_CreateProperly()
        {
            var id = Guid.NewGuid();

            decimal amount = 1.00m;

            var expense = new Expense(id, amount);

            Assert.NotNull(expense);
            Assert.Equal(id, expense.Id);
            Assert.Equal(amount, expense.Amount);
        }

        [Fact]
        public void Should_BeEqual_GivenSameId()
        {
            var id1 = Guid.NewGuid();

            var expense1 = new Expense(id1, default);
            var expense2 = new Expense(id1, default);

            Assert.True(expense1.Equals(expense2));
            Assert.True(expense1 == expense2);
            Assert.Equal(expense1, expense2);
        }

        [Fact]
        public void Should_NotBeEqual_GivenDifferentIds()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var expense1 = new Expense(id1, default);
            var expense2 = new Expense(id2, default);

            Assert.False(expense1.Equals(expense2));
            Assert.False(expense1 == expense2);
            Assert.NotEqual(expense1, expense2);
        }
    }
}
