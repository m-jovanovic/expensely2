using System;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;
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

            Assert.Equal(password1, password2);
            Assert.Equal(password2, password1);
            Assert.True(password1 == password2);
            Assert.True(password2 == password1);
            Assert.Equal(password1.GetHashCode(), password2.GetHashCode());
            Assert.Equal(password2.GetHashCode(), password1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_password_values_are_not_equal()
        {
            Password password1 = Password.Create("123aA!1").Value();
            Password password2 = Password.Create("123aA!2").Value();

            Assert.NotEqual(password1, password2);
            Assert.NotEqual(password2, password1);
            Assert.True(password1 != password2);
            Assert.True(password2 != password1);
            Assert.NotEqual(password1.GetHashCode(), password2.GetHashCode());
            Assert.NotEqual(password2.GetHashCode(), password1.GetHashCode());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Create_should_fail_if_password_is_null_or_empty(string password)
        {
            Result<Password> result = Password.Create(password);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.NullOrEmpty, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
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

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.TooShort, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("A00001")]
        [InlineData("ABCD0!")]
        [InlineData("A0!!!!")]
        public void Create_should_fail_if_password_is_missing_lowercase_letter(string password)
        {
            Result<Password> result = Password.Create(password);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingLowercaseLetter, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("a0000!")]
        [InlineData("abcd0!")]
        [InlineData("a0!!!!")]
        public void Create_should_fail_if_password_is_missing_uppercase_letter(string password)
        {
            Result<Password> result = Password.Create(password);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingUppercaseLetter, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("Aaaaa!")]
        public void Create_should_fail_if_password_is_missing_digit(string password)
        {
            Result<Password> result = Password.Create(password);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingDigit, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("A0000a")]
        [InlineData("0Aaaaa")]
        [InlineData("1AAAAb")]
        public void Create_should_fail_if_password_is_missing_non_alphanumeric(string password)
        {
            Result<Password> result = Password.Create(password);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingNonAlphaNumeric, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("123aA!")]
        public void Create_should_succeed_if_password_is_valid(string password)
        {
            Result<Password> result = Password.Create(password);

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
            Password value = result.Value();
            Assert.NotNull(value);
            Assert.Equal(password, value.Value);
        }
    }
}
