using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users.Validators.Email;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Validators.Email
{
    public class EmailNotNullOrEmptyValidatorTests
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

            result.Error.Should().Be(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public void Should_succeed_if_passed_valid_email()
        {
            var validator = new EmailNotNullOrEmptyValidator();

            Result result = validator.Validate(UserData.ValidEmail);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
