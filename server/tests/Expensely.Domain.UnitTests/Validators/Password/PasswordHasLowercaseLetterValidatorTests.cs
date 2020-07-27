using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Password;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.Password
{
    public class PasswordHasLowercaseLetterValidatorTests
    {
        [Theory]
        [InlineData("0123456789")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ")]
        [InlineData("._,!~@#$%&*()+-=")]
        public void Should_fail_if_passed_password_without_lowercase_letter(string password)
        {
            var validator = new PasswordHasLowercaseLetterValidator();

            Result result = validator.Validate(password);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingLowercaseLetter, result.Error);
        }

        [Theory]
        [InlineData("a")]
        [InlineData("b")]
        [InlineData("c")]
        [InlineData("d")]
        [InlineData("e")]
        [InlineData("f")]
        [InlineData("g")]
        [InlineData("h")]
        [InlineData("i")]
        [InlineData("j")]
        [InlineData("k")]
        [InlineData("l")]
        [InlineData("m")]
        [InlineData("n")]
        [InlineData("o")]
        [InlineData("p")]
        [InlineData("q")]
        [InlineData("r")]
        [InlineData("s")]
        [InlineData("t")]
        [InlineData("u")]
        [InlineData("v")]
        [InlineData("w")]
        [InlineData("x")]
        [InlineData("y")]
        [InlineData("z")]
        public void Should_succeed_if_passed_password_with_lowercase_letter(string password)
        {
            var validator = new PasswordHasLowercaseLetterValidator();

            Result result = validator.Validate(password);

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
        }
    }
}
