using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Password;
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

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.NullOrEmpty, result.Error);
        }

        [Fact]
        public void Should_succeed_if_passed_valid_password()
        {
            var validator = new PasswordNotNullOrEmptyValidator();

            Result result = validator.Validate("1");

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
        }
    }
}
