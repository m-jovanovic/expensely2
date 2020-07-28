using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Authentication.Commands.Login;
using Moq;
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

            Assert.NotNull(command);
            Assert.Equal(Email, command.Email);
            Assert.Equal(Password, command.Password);
        }

        [Fact]
        public async Task Should_call_authentication_service_with_command_values()
        {
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var commandHandler = new LoginCommandHandler(authenticationServiceMock.Object);
            var command = new LoginCommand(Email, Password);

            await commandHandler.Handle(command, default);

            authenticationServiceMock.Verify(
                x => x.LoginAsync(It.Is<string>(e => e == Email), It.Is<string>(p => p == Password)),
                Times.Once);
        }
    }
}
