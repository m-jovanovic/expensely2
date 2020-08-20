using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users.Validators.Email;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.Email
{
    public class EmailFormatValidatorTests
    {
        [Theory]
        [InlineData("")]
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
        public void Should_fail_if_passed_invalid_email(string email)
        {
            var validator = new EmailFormatValidator();

            Result result = validator.Validate(email);

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
        public void Should_succeed_if_passed_valid_email(string email)
        {
            var validator = new EmailFormatValidator();

            Result result = validator.Validate(email);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
