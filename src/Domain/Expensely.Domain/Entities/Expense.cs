using System;
using Expensely.Domain.Primitives;

namespace Expensely.Domain.Entities
{
    public class Expense : Entity, IAuditableEntity, ISoftDeletableEntity
    {
        public Expense(Guid id, decimal amount)
            : base(id)
        {
            Amount = amount;
        }

        public decimal Amount { get; private set; }

        public DateTime CreatedOnUtc { get; private set; }

        public DateTime ModifiedOnUtc { get; private set; }

        public bool Deleted { get; private set; }
    }
}
