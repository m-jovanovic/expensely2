using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Application.Commands.Expenses.CreateExpense;
using Expensely.Application.Commands.Expenses.DeleteExpense;
using Expensely.Application.Queries.Expenses.GetExpenseById;
using Expensely.Application.Queries.Expenses.GetExpenses;
using Expensely.Common.Primitives;
using Expensely.Contracts.Expenses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    [Route("api/expenses")]
    public class ExpensesController : ApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyCollection<ExpenseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenses()
        {
            var query = new GetExpensesQuery();

            IReadOnlyCollection<ExpenseDto> expenses = await Mediator.Send(query);

            if (expenses is null)
            {
                return NotFound();
            }

            return Ok(expenses);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenseById(Guid id)
        {
            var query = new GetExpenseByIdQuery(id);

            ExpenseDto? expenseDto = await Mediator.Send(query);

            if (expenseDto is null)
            {
                return NotFound();
            }

            return Ok(expenseDto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateExpense([FromBody]CreateExpenseRequest request)
        {
            var command = new CreateExpenseCommand(request.Amount, request.CurrencyId);

            Result result = await Mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteExpense(Guid id)
        {
            var command = new DeleteExpenseCommand(id);

            Result result = await Mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
    }
}
