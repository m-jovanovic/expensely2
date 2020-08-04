using System.Threading.Tasks;
using Expensely.Application.Authentication.Commands.Register;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;
using static Expensely.Tests.Common.Entities.UserData;

namespace Expensely.Application.IntegrationTests.Authentication.Commands
{
    public class RegisterCommandTests
    {
        [Fact]
        public async Task Should_register_a_user_given_valid_command()
        {
            var command = new RegisterCommand(FirstName, LastName, ValidEmail, Password, Password);

            Result result = await SendAsync(command);

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            User? user = await FindAsync<User>(u => u.Email.Value == ValidEmail);
            user.Should().NotBeNull();
        }
    }
}
