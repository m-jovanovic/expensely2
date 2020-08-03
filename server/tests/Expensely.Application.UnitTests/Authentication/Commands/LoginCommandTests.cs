using Expensely.Application.Authentication.Commands.Login;
using FluentAssertions;
using Xunit;

namespace Expensely.Application.UnitTests.Authentication.Commands
{
    public class LoginCommandTests
    {
        private const string Email = "test@test.com";
        private const string Password = "123aA!";

        [Fact]
        public void Should_construct_properly()
        {
            var command = new LoginCommand(Email, Password);

            command.Should().NotBeNull();
            command.Email.Should().Be(Email);
            command.Password.Should().Be(Password);
        }
    }
}
