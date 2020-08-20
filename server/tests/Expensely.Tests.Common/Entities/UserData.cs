using System;
using Expensely.Domain.Users;

namespace Expensely.Tests.Common.Entities
{
    public static class UserData
    {
        public static readonly FirstName ValidFirstName = FirstName.Create("John").Value();

        public static readonly LastName ValidLastName = LastName.Create("Doe").Value();

        public static readonly Email ValidEmail = Email.Create("test@expensely.net").Value();

        public static readonly string Password = "123aA!";

        public static readonly string PasswordHash = "password-hash";

        public static User CreateUser() => new User(Guid.NewGuid(), ValidFirstName, ValidLastName, ValidEmail, PasswordHash);
    }
}
