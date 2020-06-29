using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Interfaces;
using Expensely.Application.Messaging;
using Expensely.Common.Primitives;
using Expensely.Domain.Entities;

namespace Expensely.Application.Commands.Expenses.Create
{
    /// <summary>
    /// Represents the <see cref="CreateExpenseCommand"/> command-handler.
    /// </summary>
    public sealed class CreateExpenseCommandHandler : ICommandHandler<CreateExpenseCommand, Result>
    {
        private readonly IExpenseRepository _expenseRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="expenseRepository">The expense repository instance.</param>
        public CreateExpenseCommandHandler(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        /// <inheritdoc />
        public Task<Result> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = new Expense(Guid.NewGuid(), request.Amount);

            _expenseRepository.Insert(expense);

            return Task.FromResult(Result.Ok());
        }
    }
}