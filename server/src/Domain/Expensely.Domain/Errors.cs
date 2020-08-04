using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain
{
    /// <summary>
    /// Contains the domain errors.
    /// </summary>
    public static class Errors
    {
        /// <summary>
        /// Contains the authentication errors.
        /// </summary>
        public static class Authentication
        {
            public static Error PasswordsDoNotMatch => new Error("Authentication.PasswordsDoNotMatch");

            public static Error DuplicateEmail => new Error("Authentication.DuplicateEmail");

            public static Error InvalidPassword => new Error("Authentication.InvalidPassword");

            public static Error UserNotFound => new Error("Authentication.UserNotFound");
        }

        /// <summary>
        /// Contains the first name errors.
        /// </summary>
        public static class FirstName
        {
            public static Error NullOrEmpty => new Error("FirstName.NullOrEmpty");
        }

        /// <summary>
        /// Contains the last name errors.
        /// </summary>
        public static class LastName
        {
            public static Error NullOrEmpty => new Error("FirstName.NullOrEmpty");
        }

        /// <summary>
        /// Contains the email errors.
        /// </summary>
        public static class Email
        {
            public static Error NullOrEmpty => new Error("Email.NullOrEmpty");

            public static Error LongerThanAllowed => new Error("Email.LongerThanAllowed");

            public static Error IncorrectFormat => new Error("Email.IncorrectFormat");
        }

        /// <summary>
        /// Contains the password errors.
        /// </summary>
        public static class Password
        {
            public static Error NullOrEmpty => new Error("Password.NullOrEmpty");

            public static Error TooShort => new Error("Password.TooShort");

            public static Error MissingUppercaseLetter => new Error("Password.MissingUppercaseLetter");

            public static Error MissingLowercaseLetter => new Error("Password.MissingLowercaseLetter");

            public static Error MissingDigit => new Error("Password.MissingDigit");

            public static Error MissingNonAlphaNumeric => new Error("Password.MissingNonAlphaNumeric");
        }

        /// <summary>
        /// Contains the expense errors.
        /// </summary>
        public static class Expense
        {
            public static Error CurrencyIsRequired => new Error("Expense.CurrencyMissing");

            public static Error DateIsRequired => new Error("Expense.DateMissing");
        }

        /// <summary>
        /// Contains the currency errors.
        /// </summary>
        public static class Currency
        {
            public static Error NotFound => new Error("Currency.NotFound");
        }

        /// <summary>
        /// Contains the role errors.
        /// </summary>
        public static class Role
        {
            public static Error AtLeastOnePermissionIsRequired => new Error("Role.AtLeastOnePermissionIsRequired");
        }

        /// <summary>
        /// Contains the general domain errors.
        /// </summary>
        public static class General
        {
            public static Error BadRequest => new Error("General.BadRequest");

            public static Error EntityNotFound => new Error("General.EntityNotFound");

            public static Error EntityAlreadyExists => new Error("General.EntityAlreadyExists");

            public static Error ServerError => new Error("General.ServerError");
        }
    }
}
