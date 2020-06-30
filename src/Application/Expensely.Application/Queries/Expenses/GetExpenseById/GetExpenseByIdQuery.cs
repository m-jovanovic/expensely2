using System;
using Expensely.Application.Messaging;
using Expensely.Contracts.Expenses;

namespace Expensely.Application.Queries.Expenses.GetExpenseById
{
    public sealed class GetExpenseByIdQuery : IQuery<ExpenseDto?>
    {
        public GetExpenseByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
