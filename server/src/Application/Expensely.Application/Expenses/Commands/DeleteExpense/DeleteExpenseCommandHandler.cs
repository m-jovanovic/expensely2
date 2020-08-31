using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Application.Expenses.Events.ExpenseDeleted;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Transactions;
using MediatR;

namespace Expensely.Application.Expenses.Commands.DeleteExpense
{
    /// <summary>
    /// Represents the <see cref="DeleteExpenseCommand"/> handler.
    /// </summary>
    internal class DeleteExpenseCommandHandler : ICommandHandler<DeleteExpenseCommand, Result>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="expenseRepository">The expense repository.</param>
        /// <param name="mediator">The mediator.</param>
        public DeleteExpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IMediator mediator)
        {
            _expenseRepository = expenseRepository;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            Expense? expense = await _expenseRepository.GetByIdAsync(request.ExpenseId);

            if (expense is null || expense.UserId != request.UserId)
            {
                return Result.Failure(Errors.General.EntityNotFound);
            }

            _expenseRepository.Remove(expense);

            await _mediator.Publish(new ExpenseDeletedEvent(expense.Id), cancellationToken);

            return Result.Success();
        }
    }
}
