using System;
using Expensely.Domain.Transactions;
using Expensely.Tests.Common;
using FluentAssertions;
using Xunit;
using static Expensely.Tests.Common.Entities.TransactionData;

namespace Expensely.Domain.UnitTests.Transactions
{
    public sealed class IncomeTests
    {
        [Fact]
        public void Should_throw_argument_exception_if_id_is_empty()
        {
            Action action = () => new Income(Guid.Empty, Guid.NewGuid(), Name, PlusOne, GetDate());

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("id");
        }

        [Fact]
        public void Should_throw_argument_exception_if_user_id_is_empty()
        {
            Action action = () => new Income(Guid.NewGuid(), Guid.Empty, Name, MinusOne, GetDate());

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("userId");
        }

        [Fact]
        public void Should_throw_argument_exception_if_name_is_empty()
        {
            Action action = () => new Income(Guid.NewGuid(), Guid.NewGuid(), string.Empty, PlusOne, DateTime.Now);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("name");
        }

        [Fact]
        public void Should_throw_argument_exception_if_money_is_empty()
        {
            Action action = () => new Income(Guid.NewGuid(), Guid.NewGuid(), Name, Money.None, DateTime.Now);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("money");
        }

        [Fact]
        public void Should_throw_argument_exception_if_money_is_negative()
        {
            Action action = () => new Income(Guid.NewGuid(), Guid.NewGuid(), Name, MinusOne, DateTime.Now);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be(nameof(Money.Amount));
        }

        [Fact]
        public void Should_throw_argument_exception_if_date_is_empty()
        {
            Action action = () => new Income(Guid.NewGuid(), Guid.NewGuid(), Name, MinusOne, default);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("occurredOn");
        }

        [Fact]
        public void Should_construct_properly()
        {
            var id = Guid.NewGuid();
            DateTime date = GetDate();

            var income = new Income(id, Guid.NewGuid(), Name, PlusOne, date);

            income.Should().NotBeNull();
            income.Id.Should().Be(id);
            income.Name.Should().Be(Name);
            income.Money.Amount.Should().Be(PlusOne.Amount);
            income.OccurredOn.Should().Be(date.Date);
            income.TransactionType.Should().Be(TransactionType.Income);
        }

        [Fact]
        public void Should_be_equal_with_income_with_same_id()
        {
            var id = Guid.NewGuid();
            DateTime date = GetDate();

            var income1 = new Income(id, Guid.NewGuid(), Name, PlusOne, date);
            var income2 = new Income(id, Guid.NewGuid(), Name, PlusOne, date);

            income1.Should().Be(income2);
            income2.Should().Be(income1);
            (income1 == income2).Should().BeTrue();
            (income2 == income1).Should().BeTrue();
            income1.GetHashCode().Should().Be(income2.GetHashCode());
            income2.GetHashCode().Should().Be(income1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_with_income_with_different_id()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            DateTime date = GetDate();

            var income1 = new Income(id1, Guid.NewGuid(), Name, PlusOne, date);
            var income2 = new Income(id2, Guid.NewGuid(), Name, PlusOne, date);

            income1.Should().NotBe(income2);
            income2.Should().NotBe(income1);
            (income1 != income2).Should().BeTrue();
            (income2 != income1).Should().BeTrue();
            income1.GetHashCode().Should().NotBe(income2.GetHashCode());
            income2.GetHashCode().Should().NotBe(income1.GetHashCode());
        }

        private static DateTime GetDate() => Time.Now();
    }
}
