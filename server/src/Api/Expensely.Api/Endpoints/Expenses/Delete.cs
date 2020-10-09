using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Infrastructure;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Domain.Authorization;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Core.Result.Extensions;
using Expensely.Infrastructure.Authentication.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expensely.Api.Endpoints.Expenses
{
    public sealed class Delete : AsyncEndpoint
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        public Delete(IMediator mediator, IUserIdentifierProvider userIdentifierProvider)
            : base(mediator) =>
            _userIdentifierProvider = userIdentifierProvider;

        [HttpDelete(ApiRoutes.Expenses.DeleteExpense)]
        [HasPermission(Permission.ExpenseDelete)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteExpense(Guid id) =>
            await Result.Success(new DeleteExpenseCommand(id, _userIdentifierProvider.UserId))
                .Bind(command => Mediator.Send(command))
                .Match(NoContent, _ => NotFound());
    }
}
