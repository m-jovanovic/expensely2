using System.Threading.Tasks;
using Expensely.Application.Exceptions;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;
using static Expensely.Tests.Common.Commands.Authentication.RegisterCommandData;

namespace Expensely.Application.IntegrationTests.Authentication.Commands
{
    public class RegisterCommandTests
    {
        [Fact]
        public void Should_throw_validation_exception_if_first_name_is_empty()
        {
            var command = CreateCommandWithEmptyFirstName();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.FirstName.NullOrEmpty);
        }

        [Fact]
        public void Should_throw_validation_exception_if_last_name_is_empty()
        {
            var command = CreateCommandWithEmptyLastName();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.LastName.NullOrEmpty);
        }

        [Fact]
        public void Should_throw_validation_exception_if_email_is_empty()
        {
            var command = CreateCommandWithEmptyEmail();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public void Should_throw_validation_exception_if_password_is_empty()
        {
            var command = CreateCommandWithEmptyPassword();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Password.NullOrEmpty);
        }

        [Fact]
        public void Should_throw_validation_exception_if_confirm_password_is_empty()
        {
            var command = CreateCommandWithEmptyConfirmPassword();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Password.NullOrEmpty);
        }

        [Fact]
        public void Should_throw_validation_exception_if_password_and_confirm_password_dont_match()
        {
            var command = CreateCommandWithPasswordAndConfirmPasswordNotMatching();

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Throw<ValidationException>()
                .And.ErrorCodes.Should().Contain(Errors.Authentication.PasswordsDoNotMatch);
        }

        [Fact]
        public async Task Should_register_a_user_given_valid_command()
        {
            var command = CreateValidCommand();

            Result result = await SendAsync(command);

            result.IsSuccess.Should().BeTrue();
            User? user = await FindAsync<User>(u => u.Email.Value == command.Email);
            user.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_fail_if_user_with_email_already_exists()
        {
            const string email = "test-unique@expesnely.net";
            var command = CreateValidCommand(email);

            await SendAsync(command);

            Result result = await SendAsync(command);

            result.Error.Should().Be(Errors.Authentication.DuplicateEmail);
        }
    }
}
