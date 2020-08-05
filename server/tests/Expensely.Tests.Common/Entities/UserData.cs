using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;

namespace Expensely.Tests.Common.Entities
{
    public static class UserData
    {
        public static readonly string FirstName = "John";

        public static readonly string LastName = "Doe";

        public static readonly Email ValidEmail = Email.Create("test@expensely.net").Value();

        public static readonly string Password = "123aA!";

        public static readonly string PasswordHash = "password-hash";

        public static User CreateUser() => new User(Guid.NewGuid(), FirstName, LastName, ValidEmail, PasswordHash);
    }
}
