using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain.Core.Primitives;
using Expensely.Infrastructure.Authorization;
using Expensely.Infrastructure.Authorization.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    public class ExpensesController : ApiController
    {
        [HttpGet(ApiRoutes.Expenses.GetExpenses)]
        [HasPermission(Permission.ExpenseRead)]
        [ProducesResponseType(typeof(IReadOnlyCollection<ExpenseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenses()
        {
            var query = new GetExpensesQuery();

            IReadOnlyCollection<ExpenseResponse> expenses = await Mediator.Send(query);

            if (expenses is null)
            {
                return NotFound();
            }

            return Ok(expenses);
        }

        [HttpGet(ApiRoutes.Expenses.GetExpenseById)]
        [HasPermission(Permission.ExpenseRead)]
        [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenseById(Guid id)
        {
            var query = new GetExpenseByIdQuery(id);

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
        public async Task<IActionResult> CreateExpense([FromBody]CreateExpenseRequest request)
        {
            var command = new CreateExpenseCommand(request.Name, request.Amount, request.CurrencyId, request.Date);

            Result<EntityCreatedResponse> result = await Mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            EntityCreatedResponse entityCreatedResponse = result.Value();

            return CreatedAtAction(nameof(GetExpenseById), new { id = entityCreatedResponse.Id }, entityCreatedResponse);
        }

        [HttpDelete(ApiRoutes.Expenses.DeleteExpense)]
        [HasPermission(Permission.ExpenseRemove)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var command = new DeleteExpenseCommand(id);

            Result result = await Mediator.Send(command);

            if (result.IsFailure)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
