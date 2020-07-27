using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Password;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.Password
{
    public class PasswordHasDigitValidatorTests
    {
        [Theory]
        [InlineData("abcde")]
        [InlineData("Abcde")]
        [InlineData("!!!!!")]
        [InlineData("t_e_s_t")]
        public void Should_fail_if_passed_password_without_digit(string password)
        {
            var validator = new PasswordHasDigitValidator();

            Result result = validator.Validate(password);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingDigit, result.Error);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("2")]
        [InlineData("3")]
        [InlineData("4")]
        [InlineData("5")]
        [InlineData("6")]
        [InlineData("7")]
        [InlineData("8")]
        [InlineData("9")]
        public void Should_succeed_if_passed_password_with_digit(string password)
        {
            var validator = new PasswordHasDigitValidator();

            Result result = validator.Validate(password);

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
        }
    }
}
