using System;
using System.Threading.Tasks;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.ExpensesController
{
    public class DeleteExpenseTests
    {
        [Fact]
        public async Task Delete_expense_should_return_not_found_if_command_returns_failure_result()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteExpenseCommand>(), default))
                .ReturnsAsync(Result.Fail(Errors.General.EntityNotFound));
            var controller = new Api.Controllers.ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.DeleteExpense(Guid.NewGuid());

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_expense_should_return_no_content_if_command_returns_success_result()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteExpenseCommand>(), default))
                .ReturnsAsync(Result.Ok);
            var controller = new Api.Controllers.ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.DeleteExpense(Guid.NewGuid());

            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_expense_should_send_valid_command()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteExpenseCommand>(), default))
                .ReturnsAsync(Result.Ok);
            var controller = new Api.Controllers.ExpensesController(mediatorMock.Object);
            var expenseId = Guid.NewGuid();

            await controller.DeleteExpense(expenseId);

            mediatorMock.Verify(x => x.Send(It.Is<DeleteExpenseCommand>(c => c.ExpenseId == expenseId), default), Times.Once);
        }
    }
}
