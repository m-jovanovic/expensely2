using Expensely.Domain.Core;
using Expensely.Domain.Users.Validators.FirstName;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Users.Validators.FirstName
{
    public class FirstNameMaxLengthValidatorTests
    {
        [Fact]
        public void Should_fail_if_passed_longer_than_allowed_first_name()
        {
            var validator = new FirstNameMaxLengthValidator();
            string firstName = new string('*', Domain.Users.FirstName.MaxLength + 1);

            Result result = validator.Validate(firstName);

            result.Error.Should().Be(Errors.FirstName.LongerThanAllowed);
        }

        [Fact]
        public void Should_succeed_if_passed_shorter_than_allowed_first_name()
        {
            var validator = new FirstNameMaxLengthValidator();
            string firstName = new string('*', Domain.Users.FirstName.MaxLength);

            Result result = validator.Validate(firstName);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
