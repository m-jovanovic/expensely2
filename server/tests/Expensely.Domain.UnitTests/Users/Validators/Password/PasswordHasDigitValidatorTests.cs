using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users.Validators.Password;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Users.Validators.Password
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
            
            result.Error.Should().Be(Errors.Password.MissingDigit);
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

            result.IsSuccess.Should().BeTrue();
        }
    }
}
