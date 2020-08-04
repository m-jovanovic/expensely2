using Expensely.Application.Authentication.Commands.Register;
using static Expensely.Tests.Common.Entities.UserData;

namespace Expensely.Tests.Common.Commands.Authentication
{
    public static class RegisterCommandData
    {
        public static RegisterCommand CreateValidCommand(string? email = null) =>
            new RegisterCommand(FirstName, LastName, email ?? ValidEmail, Password, Password);

        public static RegisterCommand CreateCommandWithEmptyFirstName()
            => new RegisterCommand(string.Empty, LastName, ValidEmail, Password, Password);

        public static RegisterCommand CreateCommandWithEmptyLastName()
            => new RegisterCommand(FirstName, string.Empty, ValidEmail, Password, Password);

        public static RegisterCommand CreateCommandWithEmptyEmail()
            => new RegisterCommand(FirstName, LastName, string.Empty, Password, Password);

        public static RegisterCommand CreateCommandWithEmptyPassword()
            => new RegisterCommand(FirstName, LastName, ValidEmail, string.Empty, Password);

        public static RegisterCommand CreateCommandWithEmptyConfirmPassword()
            => new RegisterCommand(FirstName, LastName, ValidEmail, Password, string.Empty);

        public static RegisterCommand CreateCommandWithPasswordAndConfirmPasswordNotMatching()
            => new RegisterCommand(FirstName, LastName, ValidEmail, Password, $"{Password}!");
    }
}
