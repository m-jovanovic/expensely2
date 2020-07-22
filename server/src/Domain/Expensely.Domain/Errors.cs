using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain
{
    public static class Errors
    {
        public static class Authentication
        {
            public static Error PasswordsDoNotMatch => new Error("Authentication.PasswordsDoNotMatch");

            public static Error DuplicateEmail => new Error("Authentication.DuplicateEmail");

            public static Error InvalidPassword => new Error("Authentication.InvalidPassword");

            public static Error UserNotFound => new Error("Authentication.UserNotFound");
        }

        public static class Email
        {
            public static Error NullOrEmpty => new Error("Email.NullOrEmpty");

            public static Error LongerThanAllowed => new Error("Email.LongerThanAllowed");

            public static Error IncorrectFormat => new Error("Email.IncorrectFormat");
        }

        public static class Password
        {
            public static Error NullOrEmpty => new Error("Password.NullOrEmpty");

            public static Error TooShort => new Error("Password.TooShort");

            public static Error MissingUppercaseLetter => new Error("Password.MissingUppercaseLetter");

            public static Error MissingLowercaseLetter => new Error("Password.MissingLowercaseLetter");

            public static Error MissingDigit => new Error("Password.MissingDigit");

            public static Error MissingNonAlphaNumeric => new Error("Password.MissingNonAlphaNumeric");
        }

        public static class Currency
        {
            public static Error NotFound => new Error("Currency.NotFound");
        }

        public static class Role
        {
            public static Error AtLeastOnePermissionIsRequired => new Error("Role.AtLeastOnePermissionIsRequired");
        }

        public static class General
        {
            public static Error NotFound => new Error("Entity.NotFound");

            public static Error AlreadyExists => new Error("Entity.AlreadyExists");
        }
    }
}
