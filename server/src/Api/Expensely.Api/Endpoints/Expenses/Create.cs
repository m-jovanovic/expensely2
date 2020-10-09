using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Domain;
using Expensely.Domain.Authorization;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Core.Result.Extensions;
using Expensely.Infrastructure.Authentication.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Endpoints.Expenses
{
    public class Create : AsyncEndpoint
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        public Create(IMediator mediator, IUserIdentifierProvider userIdentifierProvider)
            : base(mediator) =>
            _userIdentifierProvider = userIdentifierProvider;

        [HttpPost(ApiRoutes.Expenses.CreateExpense)]
        [HasPermission(Permission.ExpenseCreate)]
        [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Handle([FromBody] CreateExpenseRequest? request) =>
            await Result.Create(request, Errors.General.BadRequest)
                .Ensure(value => value.UserId == _userIdentifierProvider.UserId, Errors.General.BadRequest)
                .Map(value => new CreateExpenseCommand(
                    value.UserId,
                    value.Name,
                    value.Amount,
                    value.CurrencyId,
                    value.Date))
                .Bind(command => Mediator.Send(command))
                .Match(
                    entityCreated => CreatedAtAction(
                        nameof(GetById.Handle),
                        nameof(GetById),
                        new { id = entityCreated.Id },
                        entityCreated),
                    BadRequest);
    }
}
