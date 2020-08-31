using System.Threading.Tasks;
using Expensely.Application.Authentication.Commands.Login;
using Expensely.Application.Contracts.Authentication;
using Expensely.Application.Core.Exceptions;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Tests.Common.Commands.Authentication;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;
using static Expensely.Tests.Common.Commands.Authentication.LoginCommandData;

namespace Expensely.Application.IntegrationTests.Authentication.Commands
{
    public class LoginCommandTests
    {
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
        public async Task Should_fail_if_user_with_email_is_not_found()
        {
            const string email = "test-not-found@expesnely.net";
            var command = CreateValidCommand(email);

            Result<TokenResponse> result = await SendAsync(command);

            result.Error.Should().Be(Errors.Authentication.InvalidEmailOrPassword);
        }

        [Fact]
        public async Task Should_fail_if_password_is_invalid()
        {
            const string email = "test-invalid-password@expensely.net";
            var registerCommand = RegisterCommandData.CreateValidCommand(email);
            await SendAsync(registerCommand);
            var command = new LoginCommand(email, $"{UserData.Password}!");

            Result<TokenResponse> result = await SendAsync(command);

            result.Error.Should().Be(Errors.Authentication.InvalidEmailOrPassword);
        }
        
        [Fact]
        public async Task Should_succeed_given_valid_command()
        {
            const string email = "test-success@expensely.net";
            var registerCommand = RegisterCommandData.CreateValidCommand(email);
            await SendAsync(registerCommand);
            var command = CreateValidCommand(email);

            Result<TokenResponse> result = await SendAsync(command);

            result.IsSuccess.Should().BeTrue();
            result.Value().Token.Should().NotBeEmpty();
        }
    }
}
