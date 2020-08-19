using System;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Common;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Infrastructure.Services.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Expenses
{
    public class DeleteExpenseTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly IDateTime _dateTime;

        public DeleteExpenseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _dateTime = new MachineDateTime();
        }

        [Fact]
        public async Task Delete_expense_should_return_not_found_if_command_returns_failure_result()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteExpenseCommand>(), default))
                .ReturnsAsync(Result.Fail(Errors.General.EntityNotFound));
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            IActionResult result = await controller.DeleteExpense(Guid.NewGuid());

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_expense_should_return_no_content_if_command_returns_success_result()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteExpenseCommand>(), default))
                .ReturnsAsync(Result.Ok);
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            IActionResult result = await controller.DeleteExpense(Guid.NewGuid());

            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_expense_should_send_valid_command()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<DeleteExpenseCommand>(), default))
                .ReturnsAsync(Result.Ok);
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);
            var expenseId = Guid.NewGuid();

            await controller.DeleteExpense(expenseId);

            _mediatorMock.Verify(x => x.Send(It.Is<DeleteExpenseCommand>(c => c.ExpenseId == expenseId), default), Times.Once);
        }
    }
}
