using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Password;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.Password
{
    public class PasswordNotNullOrEmptyValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Should_fail_if_passed_null_or_empty_password(string password)
        {
            var validator = new PasswordNotNullOrEmptyValidator();

            Result result = validator.Validate(password);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Password.NullOrEmpty);
        }

        [Fact]
        public void Should_succeed_if_passed_valid_password()
        {
            var validator = new PasswordNotNullOrEmptyValidator();

            Result result = validator.Validate("1");

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
        }
    }
}
