using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Domain.UnitTests.Entities
{
    public sealed class ExpenseTests
    {
        private const string Name = "Expense";
        private static readonly Money Money = new Money(decimal.Zero, Currency.Usd);

        [Fact]
        public void Should_construct_properly()
        {
            var id = Guid.NewGuid();
            DateTime date = GetDate();

            var expense = new Expense(id, Name, Money, date);

            Assert.NotNull(expense);
            Assert.Equal(id, expense.Id);
            Assert.Equal(Name, expense.Name);
            Assert.Equal(Money.Amount, expense.Money.Amount);
            Assert.Equal(date.Date, expense.Date);
        }

        [Fact]
        public void Should_be_equal_with_expense_with_same_id()
        {
            var id = Guid.NewGuid();
            DateTime date = GetDate();

            var expense1 = new Expense(id, Name, Money, date);
            var expense2 = new Expense(id, Name, Money, date);

            Assert.True(expense1.Equals(expense2));
            Assert.True(expense1 == expense2);
            Assert.Equal(expense1, expense2);
            Assert.Equal(expense1.GetHashCode(), expense2.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_with_expense_with_different_id()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            DateTime date = GetDate();

            var expense1 = new Expense(id1, Name, Money, date);
            var expense2 = new Expense(id2, Name, Money, date);

            Assert.False(expense1.Equals(expense2));
            Assert.True(expense1 != expense2);
            Assert.NotEqual(expense1, expense2);
            Assert.NotEqual(expense1.GetHashCode(), expense2.GetHashCode());
        }

        [Fact]
        public void Should_throw_argument_exception_if_id_is_empty()
        {
            Assert.Throws<ArgumentException>(() => new Expense(Guid.Empty, Name, Money, DateTime.Now));
        }

        [Fact]
        public void Should_throw_argument_exception_if_money_is_empty()
        {
            Assert.Throws<ArgumentException>(() => new Expense(Guid.NewGuid(), Name, Money.None, DateTime.Now));
        }

        [Fact]
        public void Should_throw_argument_exception_if_date_is_empty()
        {
            Assert.Throws<ArgumentException>(() => new Expense(Guid.NewGuid(), Name, Money, default));
        }

        private static DateTime GetDate() => DateTime.Now;
    }
}
