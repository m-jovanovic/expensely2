using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Email;
using Xunit;

namespace Expensely.Domain.Tests.Validators.Email
{
    public class EmailNullOrEmptyValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Should_fail_if_passed_null_or_empty_email(string email)
        {
            var validator = new EmailNotNullOrEmptyValidator();

            Result result = validator.Validate(email);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.NullOrEmpty, result.Error);
        }

        [Fact]
        public void Should_succeed_if_passed_valid_email()
        {
            var validator = new EmailNotNullOrEmptyValidator();

            Result result = validator.Validate("test@test.com");

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
        }
    }
}
