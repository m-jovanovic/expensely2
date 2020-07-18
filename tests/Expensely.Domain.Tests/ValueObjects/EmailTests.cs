using System;
using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Email;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Domain.Tests.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void EmailsShouldBeEqualIfValuesAreEqual()
        {
            Email email1 = Email.Create("test@email.test").Value()!;
            Email email2 = Email.Create("test@email.test").Value()!;

            Assert.Equal(email1, email2);
            Assert.Equal(email2, email1);
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
        public void EmailCreateResultShouldMatchExpected(string email)
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
        public void EmailCreateResultShouldMatchExpected1(string email)
        {
            Result<Email> result = Email.Create(email);

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
            Email? value = result.Value();
            Assert.NotNull(value);
            Assert.Equal(email, value.Value);
        }

        [Fact]
        public void EmailCreateShouldFailIfEmailIsNull()
        {
            Result<Email> result = Email.Create(null);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.NullOrEmpty, result.Error);
        }

        [Fact]
        public void EmailCreateShouldFailIfEmailIsEmpty()
        {
            Result<Email> result = Email.Create(string.Empty);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.NullOrEmpty, result.Error);
        }

        [Fact]
        public void EmailCreateShouldFailIfEmailIsMoreThan255CharactersLong()
        {
            string email = string.Join(
                string.Empty, Enumerable.Range(0, EmailLengthValidator.MaxEmailLength + 1).Select(x => "a"));

            Result<Email> result = Email.Create(email);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.LongerThanAllowed, result.Error);
        }
    }
}
