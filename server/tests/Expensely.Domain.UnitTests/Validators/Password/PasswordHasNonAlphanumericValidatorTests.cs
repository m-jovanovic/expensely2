using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users.Validators.Password;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.Password
{
    public class PasswordHasNonAlphanumericValidatorTests
    {
        [Theory]
        [InlineData("0123456789")]
        [InlineData("abcdefghijkmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        public void Should_fail_if_passed_password_without_symbol(string password)
        {
            var validator = new PasswordHasNonAlphanumericValidator();;

            Result result = validator.Validate(password);

            result.Error.Should().Be(Errors.Password.MissingNonAlphaNumeric);
        }

        [Theory]
        [InlineData("~")]
        [InlineData("!")]
        [InlineData("@")]
        [InlineData("#")]
        [InlineData("$")]
        [InlineData("%")]
        [InlineData("^")]
        [InlineData("&")]
        [InlineData("*")]
        [InlineData("(")]
        [InlineData(")")]
        [InlineData("_")]
        [InlineData("-")]
        [InlineData("+")]
        [InlineData("=")]
        [InlineData(";")]
        [InlineData(":")]
        [InlineData("'")]
        [InlineData("\"")]
        [InlineData("\\")]
        [InlineData("/")]
        [InlineData("|")]
        [InlineData("{")]
        [InlineData("}")]
        [InlineData("[")]
        [InlineData("]")]
        public void Should_succeed_if_passwed_password_with_symbol(string password)
        {
            var validator = new PasswordHasNonAlphanumericValidator();

            Result result = validator.Validate(password);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
