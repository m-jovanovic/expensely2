using System;
using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Email;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Domain.UnitTests.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void Should_be_equal_if_email_values_are_equal()
        {
            Email email1 = Email.Create("test@email.test").Value();
            Email email2 = Email.Create("test@email.test").Value();

            Assert.NotSame(email1, email2);
            Assert.Equal(email1, email2);
            Assert.Equal(email2, email1);
            Assert.True(email1 == email2);
            Assert.True(email2 == email1);
            Assert.Equal(email1.GetHashCode(), email2.GetHashCode());
            Assert.Equal(email2.GetHashCode(), email1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_if_email_values_are_not_equal()
        {
            Email email1 = Email.Create("test1@email.test").Value();
            Email email2 = Email.Create("test2@email.test").Value();

            Assert.NotEqual(email1, email2);
            Assert.NotEqual(email2, email1);
            Assert.True(email1 != email2);
            Assert.True(email2 != email1);
            Assert.NotEqual(email1.GetHashCode(), email2.GetHashCode());
            Assert.NotEqual(email2.GetHashCode(), email1.GetHashCode());
        }

        [Fact]
        public void Create_should_fail_if_email_is_null()
        {
            Result<Email> result = Email.Create(null);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.NullOrEmpty, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Fact]
        public void Create_should_fail_if_email_is_empty()
        {
            Result<Email> result = Email.Create(string.Empty);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.NullOrEmpty, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Fact]
        public void Create_should_fail_if_email_is_longer_than_allowed()
        {
            string email = string.Join(
                string.Empty, Enumerable.Range(0, EmailMaxLengthValidator.MaxEmailLength + 1).Select(x => "a"));

            Result<Email> result = Email.Create(email);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.LongerThanAllowed, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
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

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.IncorrectFormat, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
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

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
            Email value = result.Value();
            Assert.NotNull(value);
            Assert.Equal(email, value.Value);
        }
    }
}
