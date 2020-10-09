using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Core.Extensions;
using Expensely.Application.Transactions.Queries.GetCurrentWeekBalance;
using Expensely.Application.Transactions.Queries.GetTransactions;
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
    public class TransactionsController : ApiController
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IDateTime _dateTime;

        public TransactionsController(IMediator mediator, IUserIdentifierProvider userIdentifierProvider, IDateTime dateTime)
            : base(mediator)
        {
            _userIdentifierProvider = userIdentifierProvider;
            _dateTime = dateTime;
        }

        [HttpGet(ApiRoutes.Transactions.GetTransactions)]
        [HasPermission(Permission.TransactionRead)]
        [ProducesResponseType(typeof(TransactionListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactions(Guid userId, int limit, string? cursor) =>
            await Result.Success(new GetTransactionsQuery(userId, limit, cursor, _dateTime.UtcNow))
                .Ensure(query => query.UserId == _userIdentifierProvider.UserId, Errors.General.EntityNotFound)
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpGet(ApiRoutes.Transactions.GetCurrentWeekBalance)]
        [HasPermission(Permission.TransactionRead)]
        [ProducesResponseType(typeof(BalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentWeekBalance(Guid userId, int currencyId) =>
            await Result.Success(new GetCurrentWeekBalanceQuery(userId, currencyId, _dateTime.UtcNow.StartOfWeek()))
                .Ensure(query => query.UserId == _userIdentifierProvider.UserId, Errors.General.EntityNotFound)
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);
    }
}
