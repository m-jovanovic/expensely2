using Expensely.Application.Authentication.Commands.Login;
using static Expensely.Tests.Common.Entities.UserData;

namespace Expensely.Tests.Common.Commands.Authentication
{
    public static class LoginCommandData
    {
        public static LoginCommand CreateValidCommand(string? email = null) => new LoginCommand(email ?? ValidEmail, Password);

        public static LoginCommand CreateCommandWithEmptyEmail() => new LoginCommand(string.Empty, Password);

        public static LoginCommand CreateCommandWithEmptyPassword() => new LoginCommand(ValidEmail, string.Empty);
    }
}
