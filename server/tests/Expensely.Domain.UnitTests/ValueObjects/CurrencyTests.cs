using System.Collections.Generic;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class CurrencyTests
    {
        public static IEnumerable<object[]> GetIdenticalCurrencies()
        {
            yield return new object[] { Currency.Usd, Currency.Usd };
            yield return new object[] { Currency.Eur, Currency.Eur };
            yield return new object[] { Currency.Rsd, Currency.Rsd };
        }

        public static IEnumerable<object[]> GetDifferentCurrencies()
        {
            yield return new object[] { Currency.Usd, Currency.Eur };
            yield return new object[] { Currency.Usd, Currency.Rsd };
            yield return new object[] { Currency.Eur, Currency.Usd };
            yield return new object[] { Currency.Eur, Currency.Rsd };
            yield return new object[] { Currency.Rsd, Currency.Usd };
            yield return new object[] { Currency.Rsd, Currency.Eur };
        }

        [Theory]
        [MemberData(nameof(GetIdenticalCurrencies))]
        public void Should_BeEqual_GivenSameCurrency(Currency currency1, Currency currency2)
        {
            Assert.Equal(currency1, currency2);
            Assert.True(currency1 == currency2);
            Assert.True(currency1.Equals(currency2));
            Assert.True(currency1.GetHashCode() == currency2.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(GetDifferentCurrencies))]
        public void Should_NotBeEqual_GivenSameCurrency(Currency currency1, Currency currency2)
        {
            Assert.NotEqual(currency1, currency2);
            Assert.False(currency1 == currency2);
            Assert.False(currency1.Equals(currency2));
            Assert.False(currency1.GetHashCode() == currency2.GetHashCode());
        }

        [Fact]
        public void FromId_Should_ReturnNull_GivenNegativeCurrencyId()
        {
            int currencyId = int.MinValue;

            var currency = Currency.FromId(currencyId);

            Assert.Null(currency);
        }

        [Fact]
        public void FromId_Should_ReturnNull_GivenOutOfBoundsCurrencyId()
        {
            int currencyId = Currency.AllCurrencies.Count + 1;

            var currency = Currency.FromId(currencyId);

            Assert.Null(currency);
        }

        [Fact]
        public void FromId_ShouldReturnCurrency_GivenLowestCurrencyId()
        {
            int currencyId = 1;

            var currency = Currency.FromId(currencyId);

            Assert.NotNull(currency);
        }

        [Fact]
        public void FromId_ShouldReturnCurrency_GivenHighestCurrencyId()
        {
            int currencyId = Currency.AllCurrencies.Count;

            var currency = Currency.FromId(currencyId);

            Assert.NotNull(currency);
        }
    }
}
