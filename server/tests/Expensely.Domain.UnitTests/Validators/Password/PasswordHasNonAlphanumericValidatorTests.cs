using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Password;
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

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingNonAlphaNumeric, result.Error);
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

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
        }
    }
}
