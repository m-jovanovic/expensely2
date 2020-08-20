using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Common;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Enums;
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
        public async Task<IActionResult> GetExpenses(int limit, string? cursor)
        {
            var query = new GetExpensesQuery(_userIdentifierProvider.UserId, limit, cursor, _dateTime.UtcNow);

            ExpenseListResponse expenseListResponse = await Mediator.Send(query);

            if (expenseListResponse.Items.Count == 0)
            {
                return NotFound();
            }

            return Ok(expenseListResponse);
        }

        [HttpGet(ApiRoutes.Expenses.GetExpenseById)]
        [HasPermission(Permission.ExpenseRead)]
        [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenseById(Guid id)
        {
            var query = new GetExpenseByIdQuery(id, _userIdentifierProvider.UserId);

            ExpenseResponse? expenseDto = await Mediator.Send(query);

            if (expenseDto is null)
            {
                return NotFound();
            }

            return Ok(expenseDto);
        }

        [HttpPost(ApiRoutes.Expenses.CreateExpense)]
        [HasPermission(Permission.ExpenseCreate)]
        [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExpense([FromBody]CreateExpenseRequest? request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var command = new CreateExpenseCommand(
                _userIdentifierProvider.UserId,
                request.Name,
                request.Amount,
                request.CurrencyCode,
                request.Date);

            Result<EntityCreatedResponse> result = await Mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            EntityCreatedResponse entityCreatedResponse = result.Value();

            return CreatedAtAction(nameof(GetExpenseById), new { id = entityCreatedResponse.Id }, entityCreatedResponse);
        }

        [HttpDelete(ApiRoutes.Expenses.DeleteExpense)]
        [HasPermission(Permission.ExpenseDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var command = new DeleteExpenseCommand(id, _userIdentifierProvider.UserId);

            Result result = await Mediator.Send(command);

            if (result.IsFailure)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
