﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Abstractions.Repositories;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Expenses.Events.ExpenseCreated;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
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
        public CreateExpenseCommandHandler(IExpenseRepository expenseRepository, IMediator mediator)
        {
            _expenseRepository = expenseRepository;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Result<EntityCreatedResponse>> Handle(
            CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            var currency = Currency.FromId(request.CurrencyId);

            if (currency is null)
            {
                return Result.Fail<EntityCreatedResponse>(Errors.Currency.NotFound);
            }

            var money = new Money(request.Amount, currency);

            var expense = new Expense(Guid.NewGuid(), request.Name, money, request.Date);

            _expenseRepository.Insert(expense);

            await _mediator.Publish(new ExpenseCreatedEvent(expense.Id), cancellationToken);

            return Result.Ok(new EntityCreatedResponse(expense.Id));
        }
    }
}