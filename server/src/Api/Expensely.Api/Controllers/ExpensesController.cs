using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain;
using Expensely.Domain.Authorization;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Extensions;
using Expensely.Infrastructure.Authentication.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    public class ExpensesController : ApiController
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IDateTime _dateTime;

        public ExpensesController(IMediator mediator, IUserIdentifierProvider userIdentifierProvider, IDateTime dateTime)
            : base(mediator)
        {
            _userIdentifierProvider = userIdentifierProvider;
            _dateTime = dateTime;
        }

        [HttpGet(ApiRoutes.Expenses.GetExpenses)]
        [HasPermission(Permission.ExpenseRead)]
        [ProducesResponseType(typeof(ExpenseListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenses(Guid userId, int limit, string? cursor) =>
            await Result.Create(new GetExpensesQuery(userId, limit, cursor, _dateTime.UtcNow))
                .Ensure(query => query.UserId == _userIdentifierProvider.UserId, Errors.General.EntityNotFound)
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpGet(ApiRoutes.Expenses.GetExpenseById)]
        [HasPermission(Permission.ExpenseRead)]
        [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenseById(Guid id) =>
            await Result.Create(new GetExpenseByIdQuery(id, _userIdentifierProvider.UserId))
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpPost(ApiRoutes.Expenses.CreateExpense)]
        [HasPermission(Permission.ExpenseCreate)]
        [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseRequest? request) =>
            await Result.Create(request, Errors.General.BadRequest)
                .Map(value => new CreateExpenseCommand(
                    _userIdentifierProvider.UserId,
                    value.Name,
                    value.Amount,
                    value.CurrencyId,
                    value.Date))
                .Bind(command => Mediator.Send(command))
                .Match(
                    entityCreated => CreatedAtAction(nameof(GetExpenseById), new { id = entityCreated.Id }, entityCreated),
                    BadRequest);

        [HttpDelete(ApiRoutes.Expenses.DeleteExpense)]
        [HasPermission(Permission.ExpenseDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExpense(Guid id) =>
            await Result.Create(new DeleteExpenseCommand(id, _userIdentifierProvider.UserId))
                .Bind(command => Mediator.Send(command))
                .Match(NoContent, NotFound);
    }
}
