using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;

namespace Expensely.Authentication.Entities
{
    public class AuthenticatedUser : User
    {
        public AuthenticatedUser(string firstName, string lastName, Email email, string passwordHash)
            : base(firstName, lastName, email)
        {
            PasswordHash = passwordHash;
        }

        public string PasswordHash { get; }
    }
}
