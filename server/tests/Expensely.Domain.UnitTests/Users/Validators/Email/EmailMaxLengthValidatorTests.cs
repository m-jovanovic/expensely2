using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users.Validators.Email;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Users.Validators.Email
{
    public class EmailMaxLengthValidatorTests
    {
        [Fact]
        public void Should_fail_if_passed_longer_than_allowed_email()
        {
            var validator = new EmailMaxLengthValidator();
            string email = new string('*', Domain.Users.Email.MaxLength + 1);

            Result result = validator.Validate(email);

            result.Error.Should().Be(Errors.Email.LongerThanAllowed);
        }

        [Fact]
        public void Should_succeed_if_passed_shorter_than_allowed_email()
        {
            var validator = new EmailMaxLengthValidator();
            string email = new string('*', Domain.Users.Email.MaxLength);

            Result result = validator.Validate(email);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
