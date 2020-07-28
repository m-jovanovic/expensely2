using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.Expenses.Events.ExpenseDeleted;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Commands
{
    public class DeleteExpenseCommandTests
    {
        [Fact]
        public void Should_construct_properly()
        {
            var command = new DeleteExpenseCommand(Guid.Empty);

            Assert.NotNull(command);
            Assert.Equal(Guid.Empty, command.ExpenseId);
        }

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

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.General.EntityNotFound, result.Error);
        }

        [Fact]
        public async Task Handle_should_call_remove_on_expense_repository()
        {
            var expense = CreateExpense();
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
            var expense = CreateExpense();
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
            var expense = CreateExpense();
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == expense.Id))).ReturnsAsync(expense);
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, new Mock<IMediator>().Object);
            var command = new DeleteExpenseCommand(expense.Id);

            Result result = await commandHandler.Handle(command, default);

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
        }

        private static Expense CreateExpense()
            =>new Expense(Guid.NewGuid(), "Expense name", new Money(1.0m, Currency.Usd), DateTime.Now);
    }
}
