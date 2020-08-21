using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users.Validators.LastName;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Users.Validators.LastName
{
    public class LastNameNotNullOrEmptyValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Should_fail_if_passed_null_or_empty_last_name(string lastName)
        {
            var validator = new LastNameNotNullOrEmptyValidator();

            Result result = validator.Validate(lastName);

            result.Error.Should().Be(Errors.LastName.NullOrEmpty);
        }

        [Fact]
        public void Should_succeed_if_passed_valid_last_name()
        {
            var validator = new LastNameNotNullOrEmptyValidator();

            Result result = validator.Validate(UserData.ValidLastName);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
