using System;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class MoneyTests
    {
        private static readonly Currency _currency = Currency.AllCurrencies[0];

        [Fact]
        public void Should_ConstructProperly()
        {
            var money = new Money(0, _currency);

            Assert.Equal(0, money.Amount);
            Assert.Equal(Currency.Usd, money.Currency);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 1)]
        [InlineData(12.01049102412, 15151.12312312954190)]
        [InlineData(-13123, -123123123)]
        public void Should_AddProperly(decimal amount1, decimal amount2)
        {
            var money1 = new Money(amount1, _currency);
            var money2 = new Money(amount2, _currency);

            Money addedMoney = money1 + money2;

            Assert.Equal(addedMoney.Amount, amount1 + amount2);
            Assert.Equal(addedMoney.Currency, _currency);
        }

        [Fact]
        public void Should_ThrowInvalidOperationException_WhenAddingDifferentCurrencies()
        {
            var money1 = new Money(0, Currency.Usd);
            var money2 = new Money(0, Currency.Eur);

            Assert.Throws<InvalidOperationException>(() => money1 + money2);
        }

        [Fact]
        public void Should_CreateNewInstance_WhenAdding()
        {
            var money1 = new Money(0, _currency);
            var money2 = new Money(0, _currency);

            Money addedMoney = money1 + money2;

            Assert.NotSame(addedMoney, money1);
            Assert.NotSame(addedMoney, money2);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 1)]
        [InlineData(12.01049102412, 15151.12312312954190)]
        [InlineData(-13123, -123123123)]
        public void Should_SubtractProperly(decimal amount1, decimal amount2)
        {
            var money1 = new Money(amount1, _currency);
            var money2 = new Money(amount2, _currency);

            Money addedMoney = money1 - money2;

            Assert.Equal(addedMoney.Amount, amount1 - amount2);
            Assert.Equal(addedMoney.Currency, _currency);
        }

        [Fact]
        public void Should_ThrowInvalidOperationException_WhenSubtractingDifferentCurrencies()
        {
            var money1 = new Money(0, Currency.Usd);
            var money2 = new Money(0, Currency.Eur);

            Assert.Throws<InvalidOperationException>(() => money1 - money2);
        }

        [Fact]
        public void Should_CreateNewInstance_WhenSubtracting()
        {
            var money1 = new Money(0, _currency);
            var money2 = new Money(0, _currency);

            Money addedMoney = money1 - money2;

            Assert.NotSame(addedMoney, money1);
            Assert.NotSame(addedMoney, money2);
        }
    }
}
