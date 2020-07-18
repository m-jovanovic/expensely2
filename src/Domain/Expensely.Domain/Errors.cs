using System.Security.Cryptography.X509Certificates;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain
{
    public static class Errors
    {
        public static class Email
        {
            public static Error NullOrEmpty => new Error("Email.NullOrEmpty");

            public static Error LongerThanAllowed => new Error("Email.LongerThanAllowed");

            public static Error IncorrectFormat => new Error("Email.IncorrectFormat");
        }

        public static class Currency
        {
            public static Error NotFound => new Error("Currency.NotFound");
        }

        public static class General
        {
            public static Error NotFound => new Error("Entity.NotFound");
        }
    }
}
