using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Email;
using Xunit;

namespace Expensely.Domain.Tests.Validators.Email
{
    public class EmailLengthValidatorTests
    {
        [Fact]
        public void Should_fail_if_passed_longer_than_allowed_email()
        {
            var validator = new EmailMaxLengthValidator();
            string email = new string('*', EmailMaxLengthValidator.MaxEmailLength + 1);

            Result result = validator.Validate(email);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.LongerThanAllowed, result.Error);
        }

        [Fact]
        public void Should_succeed_if_passed_shorter_than_allowed_email()
        {
            var validator = new EmailMaxLengthValidator();
            string email = new string('*', EmailMaxLengthValidator.MaxEmailLength);

            Result result = validator.Validate(email);

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
        }
    }
}
