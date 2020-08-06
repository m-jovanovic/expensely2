using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenses;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Expenses
{
    public class GetExpensesTests
    {
        [Fact]
        public async Task Get_expenses_should_return_not_found_if_query_returns_empty_collection()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetExpensesQuery>(), default))
                .ReturnsAsync(new List<ExpenseResponse>());
            var controller = new ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.GetExpenses();

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_expenses_should_return_ok_if_query_returns_non_empty_collection()
        {
            var mediatorMock = new Mock<IMediator>();
            var expenseResponses = new List<ExpenseResponse> { new ExpenseResponse() };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetExpensesQuery>(), default))
                .ReturnsAsync(expenseResponses);
            var controller = new ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.GetExpenses();

            OkObjectResult okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            IReadOnlyCollection<ExpenseResponse> value = okObjectResult.Value.As<IReadOnlyCollection<ExpenseResponse>>();
            value.Should().NotBeNull();
            value.Should().HaveCount(expenseResponses.Count);
        }
    }
}
