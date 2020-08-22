using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Incomes;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Incomes.Commands.CreateIncome;
using Expensely.Application.Incomes.Queries.GetExpenseById;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users;
using Expensely.Infrastructure.Authentication.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Controllers
{
    public class IncomesController : ApiController
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        public IncomesController(IMediator mediator, IUserIdentifierProvider userIdentifierProvider)
            : base(mediator)
        {
            _userIdentifierProvider = userIdentifierProvider;
        }

        [HttpGet(ApiRoutes.Incomes.GetIncomeById)]
        [HasPermission(Permission.IncomeRead)]
        [ProducesResponseType(typeof(IncomeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetIncomeById(Guid id)
        {
            var query = new GetIncomeByIdQuery(id, _userIdentifierProvider.UserId);

            IncomeResponse? expenseDto = await Mediator.Send(query);

            if (expenseDto is null)
            {
                return NotFound();
            }

            return Ok(expenseDto);
        }

        [HttpPost(ApiRoutes.Incomes.CreateIncome)]
        [HasPermission(Permission.IncomeCreate)]
        [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeRequest? request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            var command = new CreateIncomeCommand(
                _userIdentifierProvider.UserId,
                request.Name,
                request.Amount,
                request.CurrencyId,
                request.Date);

            Result<EntityCreatedResponse> result = await Mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result);
            }

            EntityCreatedResponse entityCreatedResponse = result.Value();

            return CreatedAtAction(nameof(GetIncomeById), new { id = entityCreatedResponse.Id }, entityCreatedResponse);
        }
    }
}
