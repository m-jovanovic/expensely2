using System;
using Expensely.Domain.Core.Abstractions;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;

namespace Expensely.Domain.Entities
{
    public class User : Entity, IAuditableEntity, ISoftDeletableEntity
    {
        public User(string firstName, string lastName, Email email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public Email Email { get; private set; }

        public DateTime CreatedOnUtc { get; }

        public DateTime? ModifiedOnUtc { get; }

        public DateTime? DeletedOnUtc { get; }

        public bool Deleted { get; }
    }
}
