using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class LastNameTests
    {
        [Fact]
        public void Should_be_equal_if_email_values_are_equal()
        {
            LastName lastName1 = LastName.Create("Name").Value();
            LastName lastName2 = LastName.Create("Name").Value();

            lastName1.Should().NotBeSameAs(lastName2);
            lastName1.Should().Be(lastName2);
            lastName2.Should().Be(lastName1);
            (lastName1 == lastName2).Should().BeTrue();
            (lastName2 == lastName1).Should().BeTrue();
            lastName1.GetHashCode().Should().Be(lastName2.GetHashCode());
            lastName2.GetHashCode().Should().Be(lastName1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_email_values_are_not_equal()
        {
            LastName lastName1 = LastName.Create("Name1").Value();
            LastName lastName2 = LastName.Create("Name2").Value();

            lastName1.Should().NotBe(lastName2);
            lastName2.Should().NotBe(lastName1);
            (lastName1 != lastName2).Should().BeTrue();
            (lastName2 != lastName1).Should().BeTrue();
            lastName1.GetHashCode().Should().NotBe(lastName2.GetHashCode());
            lastName2.GetHashCode().Should().NotBe(lastName1.GetHashCode());
        }

        [Fact]
        public void Create_should_fail_if_last_name_is_null()
        {
            Result<LastName> result = LastName.Create(null);

            result.Error.Should().Be(Errors.LastName.NullOrEmpty);
        }

        [Fact]
        public void Create_should_fail_if_last_name_is_empty()
        {
            Result<LastName> result = LastName.Create(string.Empty);

            result.Error.Should().Be(Errors.LastName.NullOrEmpty);
        }

        [Fact]
        public void Create_should_fail_if_last_name_is_longer_than_allowed()
        {
            string email = string.Join(
                string.Empty, Enumerable.Range(0, LastName.MaxLength + 1).Select(x => "a"));

            Result<LastName> result = LastName.Create(email);

            result.Error.Should().Be(Errors.LastName.LongerThanAllowed);
        }

        [Fact]
        public void Create_should_succeed_if_last_name_is_valid()
        {
            Result<LastName> result = LastName.Create("LastName");

            result.IsSuccess.Should().BeTrue();
        }
    }
}
