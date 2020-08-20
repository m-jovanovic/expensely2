using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Email;
using Expensely.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Should_be_equal_if_email_values_are_equal()
        {
            Email email1 = Email.Create("test1@expensely.net").Value();
            Email email2 = Email.Create("test1@expensely.net").Value();

            email1.Should().NotBeSameAs(email2);
            email1.Should().Be(email2);
            email2.Should().Be(email1);
            (email1 == email2).Should().BeTrue();
            (email2 == email1).Should().BeTrue();
            email1.GetHashCode().Should().Be(email2.GetHashCode());
            email2.GetHashCode().Should().Be(email1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_email_values_are_not_equal()
        {
            Email email1 = Email.Create("test1@expensely.net").Value();
            Email email2 = Email.Create("test2@expensely.net").Value();

            email1.Should().NotBe(email2);
            email2.Should().NotBe(email1);
            (email1 != email2).Should().BeTrue();
            (email2 != email1).Should().BeTrue();
            email1.GetHashCode().Should().NotBe(email2.GetHashCode());
            email2.GetHashCode().Should().NotBe(email1.GetHashCode());
        }

        [Fact]
        public void Create_should_fail_if_email_is_null()
        {
            Result<Email> result = Email.Create(null);

            result.Error.Should().Be(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public void Create_should_fail_if_email_is_empty()
        {
            Result<Email> result = Email.Create(string.Empty);

            result.Error.Should().Be(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public void Create_should_fail_if_email_is_longer_than_allowed()
        {
            string email = string.Join(
                string.Empty, Enumerable.Range(0, Email.MaxLength + 1).Select(x => "a"));

            Result<Email> result = Email.Create(email);

            result.Error.Should().Be(Errors.Email.LongerThanAllowed);
        }

        [Theory]
        [InlineData(@"NotAnEmail")]
        [InlineData(@"@NotAnEmail")]
        [InlineData(@"""test\test""@example.com")]
        [InlineData("\"test\rtest\"@example.com")]
        [InlineData(@"""test""test""@example.com")]
        [InlineData(@".test@example.com")]
        [InlineData(@"te..st@example.com")]
        [InlineData(@"teeest.@example.com")]
        [InlineData(@".@example.com")]
        [InlineData(@"Tes T@example.com")]
        public void Create_should_fail_if_email_format_is_invalid(string email)
        {
            Result<Email> result = Email.Create(email);

            result.Error.Should().Be(Errors.Email.InvalidFormat);
        }

        [Theory]
        [InlineData(@"""test\\test""@example.com")]
        [InlineData("\"test\\\rtest\"@example.com")]
        [InlineData(@"""test\""test""@example.com")]
        [InlineData(@"test/test@example.com")]
        [InlineData(@"$A12345@example.com")]
        [InlineData(@"!def!xyz%abc@example.com")]
        [InlineData(@"_Test.Test@example.com")]
        [InlineData(@"~@example.com")]
        [InlineData(@"""Test@Test""@example.com")]
        [InlineData(@"Test.Test@example.com")]
        [InlineData(@"""Test.Test""@example.com")]
        [InlineData(@"""Test Test""@example.com")]
        public void Create_should_succeed_if_email_format_is_valid(string email)
        {
            Result<Email> result = Email.Create(email);

            result.IsSuccess.Should().BeTrue();
            Email value = result.Value();
            value.Should().NotBeNull();
            value.Value.Should().Be(email);
        }
    }
}
