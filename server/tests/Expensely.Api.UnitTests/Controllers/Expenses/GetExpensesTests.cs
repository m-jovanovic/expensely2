using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain.Transactions;
using Expensely.Infrastructure.Services.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Expenses
{
    public class GetExpensesTests
    {
        private const int Limit = 1;
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly IDateTime _dateTime;

        public GetExpensesTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(UserId);
            _dateTime = new MachineDateTime();
        }

        [Fact]
        public async Task Get_expenses_should_return_not_found_if_query_returns_empty_collection()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpensesQuery>(), default))
                .ReturnsAsync(new ExpenseListResponse(Array.Empty<ExpenseResponse>()));
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            IActionResult result = await controller.GetExpenses(Limit, null);

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_expenses_should_return_ok_if_query_returns_non_empty_response()
        {
            var expenseListResponse = new ExpenseListResponse(new List<ExpenseResponse>
            {
                new ExpenseResponse(Guid.NewGuid(), "Expense", -1.0m, Currency.Usd.Value, DateTime.Now, DateTime.Now)
            });
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpensesQuery>(), default))
                .ReturnsAsync(expenseListResponse);
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            IActionResult result = await controller.GetExpenses(Limit, null);

            OkObjectResult okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            ExpenseListResponse response = okObjectResult.Value.As<ExpenseListResponse>();
            response.Should().NotBeNull();
            response.Items.Should().HaveCount(expenseListResponse.Items.Count);
        }

        [Fact]
        public async Task Get_expenses_should_send_valid_query()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpensesQuery>(), default))
                .ReturnsAsync(new ExpenseListResponse(Array.Empty<ExpenseResponse>()));
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            await controller.GetExpenses(Limit, null);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetExpensesQuery>(q => q.UserId == UserId && q.Limit == Limit + 1), default),
                Times.Once);
        }
    }
}
