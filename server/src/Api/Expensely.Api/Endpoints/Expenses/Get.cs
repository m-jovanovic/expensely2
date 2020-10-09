using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain.Authorization;
using Expensely.Domain.Core.Maybe;
using Expensely.Domain.Core.Maybe.Extensions;
using Expensely.Infrastructure.Authentication.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Endpoints.Expenses
{
    public sealed class Get : AsyncEndpoint
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IDateTime _dateTime;

        public Get(IMediator mediator, IUserIdentifierProvider userIdentifierProvider, IDateTime dateTime)
            : base(mediator)
        {
            _userIdentifierProvider = userIdentifierProvider;
            _dateTime = dateTime;
        }

        [HttpGet(ApiRoutes.Expenses.GetExpenses)]
        [HasPermission(Permission.ExpenseRead)]
        [ProducesResponseType(typeof(ExpenseListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Handle(Guid userId, int limit, string? cursor) =>
            await Maybe<GetExpensesQuery>.From(new GetExpensesQuery(userId, limit, cursor, _dateTime.UtcNow))
                .Ensure(query => query.UserId == _userIdentifierProvider.UserId)
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);
    }
}
