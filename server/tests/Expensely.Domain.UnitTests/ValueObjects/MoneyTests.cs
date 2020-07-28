using System;
using Expensely.Domain.ValueObjects;
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
            Assert.Throws<ArgumentException>(() => new Money(1.0m, Currency.None));
        }

        [Fact]
        public void Should_construct_properly()
        {
            var money = new Money(Amount, Currency);

            Assert.Equal(Amount, money.Amount);
            Assert.Equal(Currency.Usd, money.Currency);
        }

        [Fact]
        public void Should_be_equal_if_values_are_equal()
        {
            var money1 = new Money(Amount, Currency);
            var money2 = new Money(Amount, Currency);

            Assert.NotSame(money1, money2);
            Assert.Equal(money1, money2);
            Assert.Equal(money2, money1);
            Assert.True(money1 == money2);
            Assert.True(money2 == money1);
            Assert.Equal(money1.GetHashCode(), money2.GetHashCode());
            Assert.Equal(money2.GetHashCode(), money1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_amounts_are_not_equal()
        {
            var money1 = new Money(1.0m, Currency);
            var money2 = new Money(2.0m, Currency);

            Assert.NotSame(money1, money2);
            Assert.NotEqual(money1, money2);
            Assert.NotEqual(money2, money1);
            Assert.True(money1 != money2);
            Assert.True(money2 != money1);
            Assert.NotEqual(money1.GetHashCode(), money2.GetHashCode());
            Assert.NotEqual(money2.GetHashCode(), money1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_currencies_are_not_equal()
        {
            var money1 = new Money(Amount, Currency.Usd);
            var money2 = new Money(Amount, Currency.Eur);

            Assert.NotSame(money1, money2);
            Assert.NotEqual(money1, money2);
            Assert.NotEqual(money2, money1);
            Assert.True(money1 != money2);
            Assert.True(money2 != money1);
            Assert.NotEqual(money1.GetHashCode(), money2.GetHashCode());
            Assert.NotEqual(money2.GetHashCode(), money1.GetHashCode());
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

            Assert.Equal(addedMoney.Amount, amount1 + amount2);
            Assert.Equal(addedMoney.Currency, Currency);
        }

        [Fact]
        public void Should_throw_invalid_operation_exception_when_adding_moneys_with_different_currencies()
        {
            var money1 = new Money(Amount, Currency.Usd);
            var money2 = new Money(Amount, Currency.Eur);

            Assert.Throws<InvalidOperationException>(() => money1 + money2);
        }

        [Fact]
        public void Should_create_new_money_instance_when_adding()
        {
            var money1 = new Money(Amount, Currency);
            var money2 = new Money(Amount, Currency);

            Money addedMoney = money1 + money2;

            Assert.NotSame(addedMoney, money1);
            Assert.NotSame(addedMoney, money2);
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

            Assert.Equal(addedMoney.Amount, amount1 - amount2);
            Assert.Equal(addedMoney.Currency, Currency);
        }

        [Fact]
        public void Should_throw_invalid_operation_exception_when_subtracting_moneys_with_different_currencies()
        {
            var money1 = new Money(Amount, Currency.Usd);
            var money2 = new Money(Amount, Currency.Eur);

            Assert.Throws<InvalidOperationException>(() => money1 - money2);
        }

        [Fact]
        public void Should_create_new_money_instance_when_subtracting()
        {
            var money1 = new Money(Amount, Currency);
            var money2 = new Money(Amount, Currency);

            Money addedMoney = money1 - money2;

            Assert.NotSame(addedMoney, money1);
            Assert.NotSame(addedMoney, money2);
        }
    }
}
