using System.Threading.Tasks;
using Expensely.Application.Authentication.Commands.Register;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Authentication.Commands
{
    public class RegisterCommandTests
    {
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string Email = "test@expensely.net";
        private const string Password = "123aA!";

        [Fact]
        public async Task Should_register_a_user_given_valid_command()
        {
            var command = new RegisterCommand(FirstName, LastName, Email, Password, Password);

            Result result = await SendAsync(command);

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            User? user = await FindAsync<User>(u => u.Email.Value == Email);
            user.Should().NotBeNull();
        }
    }
}
