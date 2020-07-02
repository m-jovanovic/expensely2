using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Events.Expenses.ExpenseDeleted;
using Expensely.Application.Interfaces;
using Expensely.Application.Messaging;
using Expensely.Common.Primitives;
using Expensely.Domain.Entities;
using MediatR;

namespace Expensely.Application.Commands.Expenses.DeleteExpense
{
    public class DeleteExpenseCommandHandler : ICommandHandler<DeleteExpenseCommand, Result>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMediator _mediator;

        public DeleteExpenseCommandHandler(IExpenseRepository expenseRepository, IMediator mediator)
        {
            _expenseRepository = expenseRepository;
            _mediator = mediator;
        }

        public async Task<Result> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
        {
            Expense? expense = await _expenseRepository.GetByIdAsync(request.ExpenseId);

            if (expense is null)
            {
                return Result.Fail($"The expense with the identifier {request.ExpenseId} was not found.");
            }

            _expenseRepository.Remove(expense);

            await _mediator.Publish(new ExpenseDeletedEvent(expense.Id), cancellationToken);

            return Result.Ok();
        }
    }
}
