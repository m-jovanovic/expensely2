using System;
using Expensely.Domain.Core.Abstractions;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;

namespace Expensely.Domain.Entities
{
    public class Expense : Entity, IAuditableEntity, ISoftDeletableEntity
    {
        public Expense(Guid id, string name, Money money, DateTime date)
            : this()
        {
            Id = id;
            Name = name;
            Money = money;
            Date = date.Date;
        }

        private Expense()
        {
            Name = string.Empty;
            Money = Money.Null;
        }

        public string Name { get; private set; }

        public Money Money { get; private set; }

        public DateTime Date { get; private set; }

        public DateTime CreatedOnUtc { get; }

        public DateTime? ModifiedOnUtc { get; }

        public DateTime? DeletedOnUtc { get; }

        public bool Deleted { get; }
    }
}
