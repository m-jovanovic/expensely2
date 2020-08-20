using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.FirstName;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.FirstName
{
    public class FirstNameNotNullOrEmptyValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Should_fail_if_passed_null_or_empty_first_name(string firstName)
        {
            var validator = new FirstNameNotNullOrEmptyValidator();

            Result result = validator.Validate(firstName);

            result.Error.Should().Be(Errors.FirstName.NullOrEmpty);
        }

        [Fact]
        public void Should_succeed_if_passed_valid_first_name()
        {
            var validator = new FirstNameNotNullOrEmptyValidator();

            Result result = validator.Validate(UserData.ValidFirstName);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
