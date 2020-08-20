using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.LastName;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.LastName
{
    public class LastNameMaxLengthValidatorTests
    {
        [Fact]
        public void Should_fail_if_passed_longer_than_allowed_last_name()
        {
            var validator = new LastNameMaxLengthValidator();
            string lastName = new string('*', Domain.ValueObjects.LastName.MaxLength + 1);

            Result result = validator.Validate(lastName);

            result.Error.Should().Be(Errors.LastName.LongerThanAllowed);
        }

        [Fact]
        public void Should_succeed_if_passed_shorter_than_allowed_last_name()
        {
            var validator = new LastNameMaxLengthValidator();
            string lastName = new string('*', Domain.ValueObjects.LastName.MaxLength);

            Result result = validator.Validate(lastName);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
