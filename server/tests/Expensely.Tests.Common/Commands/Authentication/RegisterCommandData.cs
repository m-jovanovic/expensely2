using Expensely.Application.Authentication.Commands.Register;
using Expensely.Domain.Users;
using Expensely.Tests.Common.Entities;
using static Expensely.Tests.Common.Entities.UserData;

namespace Expensely.Tests.Common.Commands.Authentication
{
    public static class RegisterCommandData
    {
        public static RegisterCommand CreateValidCommand(string? email = null) =>
            new RegisterCommand(ValidFirstName, ValidLastName, email ?? ValidEmail, UserData.Password, UserData.Password);

        public static RegisterCommand CreateCommandWithEmptyFirstName()
            => new RegisterCommand(FirstName.Empty, ValidLastName, ValidEmail, UserData.Password, UserData.Password);

        public static RegisterCommand CreateCommandWithEmptyLastName()
            => new RegisterCommand(ValidFirstName, LastName.Empty, ValidEmail, UserData.Password, UserData.Password);

        public static RegisterCommand CreateCommandWithEmptyEmail()
            => new RegisterCommand(ValidFirstName, ValidLastName, string.Empty, UserData.Password, UserData.Password);

        public static RegisterCommand CreateCommandWithEmptyPassword()
            => new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, string.Empty, UserData.Password);

        public static RegisterCommand CreateCommandWithEmptyConfirmPassword()
            => new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, UserData.Password, string.Empty);

        public static RegisterCommand CreateCommandWithPasswordAndConfirmPasswordNotMatching()
            => new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, UserData.Password, $"{UserData.Password}!");
    }
}
