using System;
using Expensely.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class MoneyTests
    {
        private const decimal Amount = 1.0m;
        private static readonly Currency Currency = Currency.FromId(1)!;

        [Fact]
        public void Should_throw_argument_exception_if_currency_is_missing()
        {
            Action action = () => new Money(Amount, Currency.None);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("currency");
        }

        [Fact]
        public void Should_construct_properly()
        {
            var money = new Money(Amount, Currency);

            money.Amount.Should().Be(Amount);
            money.Currency.Should().Be(Currency);
        }

        [Fact]
        public void Should_be_equal_if_values_are_equal()
        {
            var money1 = new Money(Amount, Currency);
            var money2 = new Money(Amount, Currency);

            money1.Should().NotBeSameAs(money2);
            money1.Should().Be(money2);
            money2.Should().Be(money1);
            (money1 == money2).Should().BeTrue();
            (money2 == money1).Should().BeTrue();
            money1.GetHashCode().Should().Be(money2.GetHashCode());
            money2.GetHashCode().Should().Be(money1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_amounts_are_not_equal()
        {
            var money1 = new Money(1.0m, Currency);
            var money2 = new Money(2.0m, Currency);

            money1.Should().NotBeSameAs(money2);
            money1.Should().NotBe(money2);
            money2.Should().NotBe(money1);
            (money1 != money2).Should().BeTrue();
            (money2 != money1).Should().BeTrue();
            money1.GetHashCode().Should().NotBe(money2.GetHashCode());
            money2.GetHashCode().Should().NotBe(money1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_currencies_are_not_equal()
        {
            var money1 = new Money(Amount, Currency.Usd);
            var money2 = new Money(Amount, Currency.Eur);

            money1.Should().NotBeSameAs(money2);
            money1.Should().NotBe(money2);
            money2.Should().NotBe(money1);
            (money1 != money2).Should().BeTrue();
            (money2 != money1).Should().BeTrue();
            money1.GetHashCode().Should().NotBe(money2.GetHashCode());
            money2.GetHashCode().Should().NotBe(money1.GetHashCode());
        }

        [Fact]
        public void Should_throw_invalid_operation_exception_when_adding_moneys_with_different_currencies()
        {
            var money1 = new Money(Amount, Currency.Usd);
            var money2 = new Money(Amount, Currency.Eur);

            Func<Money> action = () => money1 + money2;

            action.Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 1)]
        [InlineData(12.01049102412, 15151.12312312954190)]
        [InlineData(-13123, -123123123)]
        public void Should_add_properly(decimal amount1, decimal amount2)
        {
            var money1 = new Money(amount1, Currency);
            var money2 = new Money(amount2, Currency);

            Money addedMoney = money1 + money2;

            addedMoney.Amount.Should().Be(amount1 + amount2);
            addedMoney.Currency.Should().Be(Currency);
        }

        [Fact]
        public void Should_create_new_money_instance_when_adding()
        {
            var money1 = new Money(Amount, Currency);
            var money2 = new Money(Amount, Currency);

            Money addedMoney = money1 + money2;

            addedMoney.Should().NotBeSameAs(money1);
            addedMoney.Should().NotBeSameAs(money2);
        }


        [Fact]
        public void Should_throw_invalid_operation_exception_when_subtracting_moneys_with_different_currencies()
        {
            var money1 = new Money(Amount, Currency.Usd);
            var money2 = new Money(Amount, Currency.Eur);

            Func<Money> action = () => money1 - money2;

            action.Should().Throw<InvalidOperationException>();
        }
        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 1)]
        [InlineData(12.01049102412, 15151.12312312954190)]
        [InlineData(-13123, -123123123)]
        public void Should_subtract_properly(decimal amount1, decimal amount2)
        {
            var money1 = new Money(amount1, Currency);
            var money2 = new Money(amount2, Currency);

            Money addedMoney = money1 - money2;

            addedMoney.Amount.Should().Be(amount1 - amount2);
            addedMoney.Currency.Should().Be(Currency);
        }

        [Fact]
        public void Should_create_new_money_instance_when_subtracting()
        {
            var money1 = new Money(Amount, Currency);
            var money2 = new Money(Amount, Currency);

            Money addedMoney = money1 - money2;

            addedMoney.Should().NotBeSameAs(money1);
            addedMoney.Should().NotBeSameAs(money2);
        }
    }
}
