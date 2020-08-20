using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class FirstNameTests
    {
        [Fact]
        public void Should_be_equal_if_email_values_are_equal()
        {
            FirstName firstName1 = FirstName.Create("Name").Value();
            FirstName firstName2 = FirstName.Create("Name").Value();

            firstName1.Should().NotBeSameAs(firstName2);
            firstName1.Should().Be(firstName2);
            firstName2.Should().Be(firstName1);
            (firstName1 == firstName2).Should().BeTrue();
            (firstName2 == firstName1).Should().BeTrue();
            firstName1.GetHashCode().Should().Be(firstName2.GetHashCode());
            firstName2.GetHashCode().Should().Be(firstName1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_email_values_are_not_equal()
        {
            FirstName firstName1 = FirstName.Create("Name1").Value();
            FirstName firstName2 = FirstName.Create("Name2").Value();

            firstName1.Should().NotBe(firstName2);
            firstName2.Should().NotBe(firstName1);
            (firstName1 != firstName2).Should().BeTrue();
            (firstName2 != firstName1).Should().BeTrue();
            firstName1.GetHashCode().Should().NotBe(firstName2.GetHashCode());
            firstName2.GetHashCode().Should().NotBe(firstName1.GetHashCode());
        }

        [Fact]
        public void Create_should_fail_if_first_name_is_null()
        {
            Result<FirstName> result = FirstName.Create(null);

            result.Error.Should().Be(Errors.FirstName.NullOrEmpty);
        }

        [Fact]
        public void Create_should_fail_if_first_name_is_empty()
        {
            Result<FirstName> result = FirstName.Create(string.Empty);

            result.Error.Should().Be(Errors.FirstName.NullOrEmpty);
        }

        [Fact]
        public void Create_should_fail_if_first_name_is_longer_than_allowed()
        {
            string email = string.Join(
                string.Empty, Enumerable.Range(0, FirstName.MaxLength + 1).Select(x => "a"));

            Result<FirstName> result = FirstName.Create(email);

            result.Error.Should().Be(Errors.FirstName.LongerThanAllowed);
        }

        [Fact]
        public void Create_should_succeed_if_first_name_is_valid()
        {
            Result<FirstName> result = FirstName.Create("FirstName");

            result.IsSuccess.Should().BeTrue();
        }
    }
}
