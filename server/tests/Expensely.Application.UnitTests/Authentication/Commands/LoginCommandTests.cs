using Expensely.Application.Authentication.Commands.Login;
using FluentAssertions;
using Xunit;
using static Expensely.Tests.Common.Data.UserData;

namespace Expensely.Application.UnitTests.Authentication.Commands
{
    public class LoginCommandTests
    {
        [Fact]
        public void Should_construct_properly()
        {
            var command = new LoginCommand(ValidEmail, Password);

            command.Should().NotBeNull();
            command.Email.Should().Be(ValidEmail);
            command.Password.Should().Be(Password);
        }
    }
}
