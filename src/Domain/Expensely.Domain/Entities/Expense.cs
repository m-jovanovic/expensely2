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

        public DateTime CreatedOnUtc { get; }

        public DateTime? ModifiedOnUtc { get; }

        public bool Deleted { get; }
    }
}
