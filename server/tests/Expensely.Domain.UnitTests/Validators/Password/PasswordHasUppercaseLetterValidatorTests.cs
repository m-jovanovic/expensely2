using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Password;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.Password
{
    public class PasswordHasUppercaseLetterValidatorTests
    {
        [Theory]
        [InlineData("0123456789")]
        [InlineData("abcdefghijkmnopqrstuvwxyz")]
        [InlineData("._,!~@#$%&*()+-=")]
        public void Should_fail_if_passed_password_without_uppercase_letter(string password)
        {
            var validator = new PasswordHasUppercaseLetterValidator();

            Result result = validator.Validate(password);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingUppercaseLetter, result.Error);
        }

        [Theory]
        [InlineData("A")]
        [InlineData("B")]
        [InlineData("C")]
        [InlineData("D")]
        [InlineData("E")]
        [InlineData("F")]
        [InlineData("G")]
        [InlineData("H")]
        [InlineData("I")]
        [InlineData("J")]
        [InlineData("K")]
        [InlineData("L")]
        [InlineData("M")]
        [InlineData("N")]
        [InlineData("O")]
        [InlineData("P")]
        [InlineData("Q")]
        [InlineData("R")]
        [InlineData("S")]
        [InlineData("T")]
        [InlineData("U")]
        [InlineData("V")]
        [InlineData("W")]
        [InlineData("X")]
        [InlineData("Y")]
        [InlineData("Z")]
        public void Should_succeed_if_passed_password_with_uppercase_letter(string password)
        {
            var validator = new PasswordHasUppercaseLetterValidator();

            Result result = validator.Validate(password);

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
        }
    }
}
