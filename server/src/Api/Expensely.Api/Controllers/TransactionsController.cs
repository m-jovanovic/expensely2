using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Common;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Transactions.Queries.GetTransactions;
using Expensely.Domain.Enums;
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
    }
}
