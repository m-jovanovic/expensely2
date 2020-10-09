using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Incomes;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Incomes.Commands.CreateIncome;
using Expensely.Application.Incomes.Queries.GetExpenseById;
using Expensely.Domain;
using Expensely.Domain.Authorization;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Core.Result.Extensions;
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
        public async Task<IActionResult> GetIncomeById(Guid id) =>
            await Result.Success(new GetIncomeByIdQuery(id, _userIdentifierProvider.UserId))
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpPost(ApiRoutes.Incomes.CreateIncome)]
        [HasPermission(Permission.IncomeCreate)]
        [ProducesResponseType(typeof(EntityCreatedResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeRequest? request) =>
            await Result.Create(request, Errors.General.BadRequest)
                .Map(value => new CreateIncomeCommand(
                    _userIdentifierProvider.UserId,
                    value.Name,
                    value.Amount,
                    value.CurrencyId,
                    value.Date))
                .Bind(command => Mediator.Send(command))
                .Match(
                    entityCreated => CreatedAtAction(nameof(GetIncomeById), new { id = entityCreated.Id }, entityCreated),
                    BadRequest);
    }
}
