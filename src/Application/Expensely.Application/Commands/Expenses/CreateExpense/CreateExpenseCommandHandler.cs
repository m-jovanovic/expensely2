using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Events.Expenses.ExpenseCreated;
using Expensely.Application.Interfaces;
using Expensely.Application.Messaging;
using Expensely.Common.Primitives;
using Expensely.Domain.Entities;
using MediatR;

namespace Expensely.Application.Commands.Expenses.CreateExpense
{
    /// <summary>
    /// Represents the <see cref="CreateExpenseCommand"/> command-handler.
    /// </summary>
    public sealed class CreateExpenseCommandHandler : ICommandHandler<CreateExpenseCommand, Result>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="expenseRepository">The expense repository instance.</param>
        /// <param name="mediator">The mediator instance.</param>
        public CreateExpenseCommandHandler(IExpenseRepository expenseRepository, IMediator mediator)
        {
            _expenseRepository = expenseRepository;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var expense = new Expense(Guid.NewGuid(), request.Amount);

            _expenseRepository.Insert(expense);

            await _mediator.Publish(new ExpenseCreatedEvent(expense.Id), cancellationToken);

            return Result.Ok();
        }
    }
}