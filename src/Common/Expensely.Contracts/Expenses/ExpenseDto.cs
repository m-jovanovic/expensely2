using System;

namespace Expensely.Contracts.Expenses
{
    public sealed class ExpenseDto
    {
        public Guid Id { get; set; }

        public decimal Amount { get; set; }
        
        public DateTime CreatedOnUtc { get; set; }
        
        public DateTime? ModifiedOnUtc { get; set; }
        
        public bool Deleted { get; set; }
    }
}
