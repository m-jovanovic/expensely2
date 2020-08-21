using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Application.Incomes.Events.IncomeCreated;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Transactions;
using MediatR;

namespace Expensely.Application.Incomes.Commands.CreateIncome
{
    /// <summary>
    /// Represents the <see cref="CreateIncomeCommand"/> handler.
    /// </summary>
    internal sealed class CreateIncomeCommandHandler : ICommandHandler<CreateIncomeCommand, Result<EntityCreatedResponse>>
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIncomeCommandHandler"/> class.
        /// </summary>
        /// <param name="incomeRepository">The income repository.</param>
        /// <param name="mediator">The mediator.</param>
        public CreateIncomeCommandHandler(
            IIncomeRepository incomeRepository,
            IMediator mediator)
        {
            _incomeRepository = incomeRepository;
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<Result<EntityCreatedResponse>> Handle(
            CreateIncomeCommand request, CancellationToken cancellationToken)
        {
            var currency = Currency.FromCode(request.CurrencyCode);

            if (currency is null)
            {
                return Result.Fail<EntityCreatedResponse>(Errors.Currency.NotFound);
            }

            var money = new Money(request.Amount, currency);

            var income = new Income(Guid.NewGuid(), request.UserId, request.Name, money, request.OccurredOn);

            _incomeRepository.Insert(income);

            await _mediator.Publish(new IncomeCreatedEvent(income.Id), cancellationToken);

            return Result.Ok(new EntityCreatedResponse(income.Id));
        }
    }
}