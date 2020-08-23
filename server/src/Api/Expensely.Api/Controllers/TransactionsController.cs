using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Core.Extensions;
using Expensely.Application.Transactions.Queries.GetCurrentWeekBalance;
using Expensely.Application.Transactions.Queries.GetTransactions;
using Expensely.Domain.Users;
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
        public async Task<IActionResult> GetTransactions(int limit, string? cursor)
        {
            var query = new GetTransactionsQuery(_userIdentifierProvider.UserId, limit, cursor, _dateTime.UtcNow);

            TransactionListResponse expenseListResponse = await Mediator.Send(query);

            if (expenseListResponse.Items.Count == 0)
            {
                return NotFound();
            }

            return Ok(expenseListResponse);
        }

        [HttpGet(ApiRoutes.Transactions.GetCurrentWeekBalance)]
        [HasPermission(Permission.TransactionRead)]
        [ProducesResponseType(typeof(BalanceResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentWeekBalance(int currencyId)
        {
            if (currencyId <= 0)
            {
                return NotFound();
            }

            var query = new GetCurrentWeekBalanceQuery(
                _userIdentifierProvider.UserId,
                currencyId,
                _dateTime.UtcNow.StartOfWeek());

            BalanceResponse? response = await Mediator.Send(query);

            if (response is null)
            {
                return NotFound();
            }

            return Ok(response);
        }
    }
}
