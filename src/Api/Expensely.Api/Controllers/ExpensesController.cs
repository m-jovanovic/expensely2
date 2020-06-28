using System.Threading.Tasks;
using Expensely.Application.Commands;
using Expensely.Application.Commands.Expenses.Create;
using Expensely.Common.Primitives;
using Expensely.Contracts.Expenses;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    [Route("api/expenses")]
    public class ExpensesController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody]CreateExpenseRequestDto request)
        {
            var command = new CreateExpenseCommand(request.Amount);

            Result result = await Mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
    }
}
