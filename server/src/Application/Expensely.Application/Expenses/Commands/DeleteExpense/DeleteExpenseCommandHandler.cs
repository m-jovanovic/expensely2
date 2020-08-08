using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Abstractions.Repositories;
using Expensely.Application.Expenses.Events.ExpenseDeleted;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
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
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="expenseRepository">The expense repository.</param>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        /// <param name="mediator">The mediator.</param>
        public DeleteExpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IUserIdentifierProvider userIdentifierProvider,
            IMediator mediator)
        {
            _expenseRepository = expenseRepository;
            _userIdentifierProvider = userIdentifierProvider;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            Expense? expense = await _expenseRepository.GetByIdAsync(request.ExpenseId);

            if (expense is null || expense.UserId != _userIdentifierProvider.UserId)
            {
                return Result.Fail(Errors.General.EntityNotFound);
            }

            _expenseRepository.Remove(expense);

            await _mediator.Publish(new ExpenseDeletedEvent(expense.Id), cancellationToken);

            return Result.Ok();
        }
    }
}
