using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Application.Expenses.Events.ExpenseCreated;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Exceptions;
using Expensely.Domain.Transactions;
using MediatR;

namespace Expensely.Application.Expenses.Commands.CreateExpense
{
    /// <summary>
    /// Represents the <see cref="CreateExpenseCommand"/> handler.
    /// </summary>
    internal sealed class CreateExpenseCommandHandler : ICommandHandler<CreateExpenseCommand, Result<EntityCreatedResponse>>
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommandHandler"/> class.
        /// </summary>
        /// <param name="expenseRepository">The expense repository.</param>
        /// <param name="mediator">The mediator.</param>
        public CreateExpenseCommandHandler(
            IExpenseRepository expenseRepository,
            IMediator mediator)
        {
            _expenseRepository = expenseRepository;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Result<EntityCreatedResponse>> Handle(
            CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            Currency currency;

            try
            {
                currency = Currency.FromValue(request.CurrencyId);
            }
            catch (InvalidEnumerationException)
            {
                return Result.Failure<EntityCreatedResponse>(Errors.Currency.NotFound);
            }

            var money = new Money(request.Amount, currency);

            var expense = new Expense(Guid.NewGuid(), request.UserId, request.Name, money, request.OccurredOn);

            _expenseRepository.Insert(expense);

            await _mediator.Publish(new ExpenseCreatedEvent(expense.Id), cancellationToken);

            return Result.Success(new EntityCreatedResponse(expense.Id));
        }
    }
}