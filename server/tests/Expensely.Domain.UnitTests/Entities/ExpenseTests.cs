using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Expensely.Tests.Common;
using FluentAssertions;
using Xunit;
using static Expensely.Tests.Common.Entities.ExpenseData;

namespace Expensely.Domain.UnitTests.Entities
{
    public sealed class ExpenseTests
    {
        [Fact]
        public void Should_throw_argument_exception_if_id_is_empty()
        {
            Action action = () => new Expense(Guid.Empty, Guid.NewGuid(), EmptyName, Zero, GetDate());

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("id");
        }

        [Fact]
        public void Should_throw_argument_exception_if_user_id_is_empty()
        {
            Action action = () => new Expense(Guid.NewGuid(), Guid.Empty, EmptyName, Zero, GetDate());

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userId");
        }

        [Fact]
        public void Should_throw_argument_exception_if_money_is_empty()
        {
            Action action = () => new Expense(Guid.NewGuid(), Guid.NewGuid(), EmptyName, Money.None, DateTime.Now);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("money");
        }

        [Fact]
        public void Should_throw_argument_exception_if_date_is_empty()
        {
            Action action = () => new Expense(Guid.NewGuid(), Guid.NewGuid(), EmptyName, Zero, default);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("date");
        }

        [Fact]
        public void Should_construct_properly()
        {
            var id = Guid.NewGuid();
            DateTime date = GetDate();

            var expense = new Expense(id, Guid.NewGuid(), Name, Zero, date);

            expense.Should().NotBeNull();
            expense.Id.Should().Be(id);
            expense.Name.Should().Be(Name);
            expense.Money.Amount.Should().Be(Zero.Amount);
            expense.Date.Should().Be(date.Date);
        }

        [Fact]
        public void Should_be_equal_with_expense_with_same_id()
        {
            var id = Guid.NewGuid();
            DateTime date = GetDate();

            var expense1 = new Expense(id, Guid.NewGuid(), Name, Zero, date);
            var expense2 = new Expense(id, Guid.NewGuid(), Name, Zero, date);

            expense1.Should().Be(expense2);
            expense2.Should().Be(expense1);
            (expense1 == expense2).Should().BeTrue();
            (expense2 == expense1).Should().BeTrue();
            expense1.GetHashCode().Should().Be(expense2.GetHashCode());
            expense2.GetHashCode().Should().Be(expense1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_with_expense_with_different_id()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            DateTime date = GetDate();

            var expense1 = new Expense(id1, Guid.NewGuid(), Name, Zero, date);
            var expense2 = new Expense(id2, Guid.NewGuid(), Name, Zero, date);

            expense1.Should().NotBe(expense2);
            expense2.Should().NotBe(expense1);
            (expense1 != expense2).Should().BeTrue();
            (expense2 != expense1).Should().BeTrue();
            expense1.GetHashCode().Should().NotBe(expense2.GetHashCode());
            expense2.GetHashCode().Should().NotBe(expense1.GetHashCode());
        }

        private static DateTime GetDate() => Time.Now();
    }
}
