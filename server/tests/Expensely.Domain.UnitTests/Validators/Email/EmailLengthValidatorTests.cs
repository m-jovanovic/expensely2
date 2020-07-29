using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Email;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.Email
{
    public class EmailLengthValidatorTests
    {
        [Fact]
        public void Should_fail_if_passed_longer_than_allowed_email()
        {
            var validator = new EmailMaxLengthValidator();
            string email = new string('*', EmailMaxLengthValidator.MaxEmailLength + 1);

            Result result = validator.Validate(email);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Email.LongerThanAllowed);
        }

        [Fact]
        public void Should_succeed_if_passed_shorter_than_allowed_email()
        {
            var validator = new EmailMaxLengthValidator();
            string email = new string('*', EmailMaxLengthValidator.MaxEmailLength);

            Result result = validator.Validate(email);

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
        }
    }
}
