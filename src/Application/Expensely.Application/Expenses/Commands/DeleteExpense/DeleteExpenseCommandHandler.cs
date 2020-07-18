﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Expenses.Events.ExpenseDeleted;
using Expensely.Application.Interfaces;
using Expensely.Application.Messaging;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using MediatR;

namespace Expensely.Application.Expenses.Commands.DeleteExpense
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
                return Result.Fail(Errors.General.NotFound);
            }

            _expenseRepository.Remove(expense);

            await _mediator.Publish(new ExpenseDeletedEvent(expense.Id), cancellationToken);

            return Result.Ok();
        }
    }
}
