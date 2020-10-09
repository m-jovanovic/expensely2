using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Domain.Authorization;
using Expensely.Domain.Core.Maybe;
using Expensely.Domain.Core.Maybe.Extensions;
using Expensely.Infrastructure.Authentication.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Endpoints.Expenses
{
    public sealed class GetById : AsyncEndpoint
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        public GetById(IMediator mediator, IUserIdentifierProvider userIdentifierProvider)
            : base(mediator) =>
            _userIdentifierProvider = userIdentifierProvider;

        [HttpGet(ApiRoutes.Expenses.GetExpenseById)]
        [HasPermission(Permission.ExpenseRead)]
        [ProducesResponseType(typeof(ExpenseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Handle(Guid id) =>
            await Maybe<GetExpenseByIdQuery>.From(new GetExpenseByIdQuery(id, _userIdentifierProvider.UserId))
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);
    }
}
