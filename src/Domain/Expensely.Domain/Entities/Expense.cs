using System;
using Expensely.Domain.Core.Abstractions;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;

namespace Expensely.Domain.Entities
{
    public class Expense : Entity, IAuditableEntity, ISoftDeletableEntity
    {
        public Expense(Guid id, Money money)
            : this()
        {
            Id = id;
            Money = money;
        }

        private Expense()
        {
            Money = Money.Null;
        }

        public Money Money { get; private set; }

        public DateTime CreatedOnUtc { get; }

        public DateTime? ModifiedOnUtc { get; }

        public bool Deleted { get; }
    }
}
