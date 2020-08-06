using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.ExpensesController
{
    public class GetExpenseByIdTests
    {
        [Fact]
        public async Task Get_expense_by_id_should_return_not_found_if_query_returns_null()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetExpenseByIdQuery>(), default))
                .ReturnsAsync((ExpenseResponse?)null);
            var controller = new Api.Controllers.ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.GetExpenseById(Guid.NewGuid());

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_expense_by_id_should_return_ok_if_query_returns_expense_response()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetExpenseByIdQuery>(), default))
                .ReturnsAsync(new ExpenseResponse());
            var controller = new Api.Controllers.ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.GetExpenseById(Guid.NewGuid());

            OkObjectResult okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            ExpenseResponse? value = okObjectResult.Value.As<ExpenseResponse?>();
            value.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_expense_by_id_should_send_valid_query()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new Api.Controllers.ExpensesController(mediatorMock.Object);
            var expenseId = Guid.NewGuid();

            await controller.GetExpenseById(expenseId);

            mediatorMock.Verify(x => x.Send(It.Is<GetExpenseByIdQuery>(q => q.ExpenseId == expenseId), default), Times.Once);
        }
    }
}
