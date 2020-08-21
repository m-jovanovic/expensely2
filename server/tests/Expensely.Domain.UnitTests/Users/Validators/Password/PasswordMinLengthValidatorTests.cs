using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users.Validators.Password;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Users.Validators.Password
{
    public class PasswordMinLengthValidatorTests
    {
        [Theory]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        public void Should_fail_if_passed_shorter_than_allowed_password(string password)
        {
            var validator = new PasswordMinLengthValidator();

            Result result = validator.Validate(password);

            result.Error.Should().Be(Errors.Password.TooShort);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("123456test")]
        public void Should_succeed_if_passed_longer_than_allowed_password(string password)
        {
            var validator = new PasswordMinLengthValidator();

            Result result = validator.Validate(password);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
