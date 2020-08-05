using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Repositories;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.Expenses.Events.ExpenseDeleted;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Commands
{
    public class DeleteExpenseCommandTests
    {
        [Fact]
        public async Task Handle_should_call_get_by_id_async_on_expense_repository()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, new Mock<IMediator>().Object);
            var command = new DeleteExpenseCommand(Guid.NewGuid());

            await commandHandler.Handle(command, default);

            expenseRepositoryMock.Verify(x => x.GetByIdAsync(It.Is<Guid>(g => g == command.ExpenseId)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_fail_if_expense_repository_returns_null()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Expense?)null);
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, new Mock<IMediator>().Object);
            var command = new DeleteExpenseCommand(Guid.Empty);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.General.EntityNotFound);
        }

        [Fact]
        public async Task Handle_should_call_remove_on_expense_repository()
        {
            var expense = ExpenseData.CreateExpense();
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == expense.Id))).ReturnsAsync(expense);
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, new Mock<IMediator>().Object);
            var command = new DeleteExpenseCommand(expense.Id);

            await commandHandler.Handle(command, default);

            expenseRepositoryMock.Verify(x => x.Remove(It.Is<Expense>(e => e == expense)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_publish_expense_deleted_event()
        {
            var expense = ExpenseData.CreateExpense();
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == expense.Id))).ReturnsAsync(expense);
            var mediatorMock = new Mock<IMediator>();
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, mediatorMock.Object);
            var command = new DeleteExpenseCommand(expense.Id);

            await commandHandler.Handle(command, default);

            mediatorMock.Verify(
                x => x.Publish(It.Is<ExpenseDeletedEvent>(e => e.ExpenseId == expense.Id), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_should_complete_successfully_if_command_is_valid()
        {
            var expense = ExpenseData.CreateExpense();
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == expense.Id))).ReturnsAsync(expense);
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, new Mock<IMediator>().Object);
            var command = new DeleteExpenseCommand(expense.Id);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
        }
    }
}
