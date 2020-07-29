using System;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class PasswordTests
    {
        [Fact]
        public void Should_be_equal_if_password_values_are_equal()
        {
            Password password1 = Password.Create("123aA!").Value();
            Password password2 = Password.Create("123aA!").Value();

            password1.Should().NotBeSameAs(password2);
            password1.Should().Be(password2);
            password2.Should().Be(password1);
            (password1 == password2).Should().BeTrue();
            (password2 == password1).Should().BeTrue();
            password1.GetHashCode().Should().Be(password2.GetHashCode());
            password2.GetHashCode().Should().Be(password1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_password_values_are_not_equal()
        {
            Password password1 = Password.Create("123aA!1").Value();
            Password password2 = Password.Create("123aA!2").Value();

            password1.Should().NotBeSameAs(password2);
            password1.Should().NotBe(password2);
            password2.Should().NotBe(password1);
            (password1 != password2).Should().BeTrue();
            (password2 != password1).Should().BeTrue();
            password1.GetHashCode().Should().NotBe(password2.GetHashCode());
            password2.GetHashCode().Should().NotBe(password1.GetHashCode());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Create_should_fail_if_password_is_null_or_empty(string password)
        {
            Result<Password> result = Password.Create(password);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Password.NullOrEmpty);
            result.Invoking(r => r.Value()).Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        public void Create_should_fail_if_password_is_shorter_than_allowed(string password)
        {
            Result<Password> result = Password.Create(password);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Password.TooShort);
            result.Invoking(r => r.Value()).Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData("A00001")]
        [InlineData("ABCD0!")]
        [InlineData("A0!!!!")]
        public void Create_should_fail_if_password_is_missing_lowercase_letter(string password)
        {
            Result<Password> result = Password.Create(password);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Password.MissingLowercaseLetter);
            result.Invoking(r => r.Value()).Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData("a0000!")]
        [InlineData("abcd0!")]
        [InlineData("a0!!!!")]
        public void Create_should_fail_if_password_is_missing_uppercase_letter(string password)
        {
            Result<Password> result = Password.Create(password);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Password.MissingUppercaseLetter);
            result.Invoking(r => r.Value()).Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData("Aaaaa!")]
        public void Create_should_fail_if_password_is_missing_digit(string password)
        {
            Result<Password> result = Password.Create(password);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Password.MissingDigit);
            result.Invoking(r => r.Value()).Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData("A0000a")]
        [InlineData("0Aaaaa")]
        [InlineData("1AAAAb")]
        public void Create_should_fail_if_password_is_missing_non_alphanumeric(string password)
        {
            Result<Password> result = Password.Create(password);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Password.MissingNonAlphaNumeric);
            result.Invoking(r => r.Value()).Should().Throw<InvalidOperationException>();
        }

        [Theory]
        [InlineData("123aA!")]
        public void Create_should_succeed_if_password_is_valid(string password)
        {
            Result<Password> result = Password.Create(password);

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            result.Invoking(r => r.Value()).Should().NotThrow();
            Password value = result.Value();
            value.Should().NotBeNull();
            value.Value.Should().Be(password);
        }
    }
}
