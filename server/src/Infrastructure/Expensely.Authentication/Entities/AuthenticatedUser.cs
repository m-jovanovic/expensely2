using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;

namespace Expensely.Authentication.Entities
{
    public class AuthenticatedUser : User
    {
        public AuthenticatedUser(Guid id, string firstName, string lastName, Email email, string passwordHash)
            : base(id, firstName, lastName, email)
        {
            PasswordHash = passwordHash;
        }

        private AuthenticatedUser()
        {
            PasswordHash = string.Empty;
        }

        public string PasswordHash { get; }
    }
}
