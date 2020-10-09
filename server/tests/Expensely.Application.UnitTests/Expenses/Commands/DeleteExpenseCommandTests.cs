using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.Expenses.Events.ExpenseDeleted;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Transactions;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Commands
{
    public class DeleteExpenseCommandTests
    {
        private readonly Mock<IExpenseRepository> _expenseRepositoryMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly Mock<IMediator> _mediatorMock;

        public DeleteExpenseCommandTests()
        {
            _expenseRepositoryMock = new Mock<IExpenseRepository>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _mediatorMock = new Mock<IMediator>();
        }

        [Fact]
        public async Task Handle_should_call_get_by_id_async_on_expense_repository()
        {
            var commandHandler = new DeleteExpenseCommandHandler(
                _expenseRepositoryMock.Object,
                _mediatorMock.Object);
            var command = new DeleteExpenseCommand(Guid.NewGuid(), Guid.NewGuid());

            await commandHandler.Handle(command, default);

            _expenseRepositoryMock.Verify(x => x.GetByIdAsync(It.Is<Guid>(g => g == command.ExpenseId)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_fail_if_expense_repository_returns_null()
        {
            _expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Expense?)null);
            var commandHandler = new DeleteExpenseCommandHandler(
                _expenseRepositoryMock.Object,
                _mediatorMock.Object);
            var command = new DeleteExpenseCommand(Guid.Empty, Guid.NewGuid());

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.General.EntityNotFound);
        }

        [Fact]
        public async Task Handle_should_fail_if_expense_repository_returns_expense_with_invalid_user_id()
        {
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(Guid.NewGuid);
            _expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(TransactionData.CreateExpense());
            var commandHandler = new DeleteExpenseCommandHandler(
                _expenseRepositoryMock.Object,
                _mediatorMock.Object);
            var command = new DeleteExpenseCommand(Guid.Empty, Guid.NewGuid());

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.General.EntityNotFound);
        }

        [Fact]
        public async Task Handle_should_call_remove_on_expense_repository()
        {
            var expense = TransactionData.CreateExpense();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(expense.UserId);
            _expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == expense.Id))).ReturnsAsync(expense);
            var commandHandler = new DeleteExpenseCommandHandler(
                _expenseRepositoryMock.Object,
                _mediatorMock.Object);
            var command = new DeleteExpenseCommand(expense.Id, expense.UserId);

            await commandHandler.Handle(command, default);

            _expenseRepositoryMock.Verify(x => x.Remove(It.Is<Expense>(e => e == expense)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_publish_expense_deleted_event()
        {
            var expense = TransactionData.CreateExpense();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(expense.UserId);
            _expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == expense.Id))).ReturnsAsync(expense);
            var commandHandler = new DeleteExpenseCommandHandler(
                _expenseRepositoryMock.Object,
                _mediatorMock.Object);
            var command = new DeleteExpenseCommand(expense.Id, expense.UserId);

            await commandHandler.Handle(command, default);

            _mediatorMock.Verify(
                x => x.Publish(It.Is<ExpenseDeletedEvent>(e => e.ExpenseId == expense.Id), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_should_complete_successfully_if_command_is_valid()
        {
            var expense = TransactionData.CreateExpense();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(expense.UserId);
            _expenseRepositoryMock.Setup(x => x.GetByIdAsync(It.Is<Guid>(g => g == expense.Id))).ReturnsAsync(expense);
            var commandHandler = new DeleteExpenseCommandHandler(
                _expenseRepositoryMock.Object,
                _mediatorMock.Object);
            var command = new DeleteExpenseCommand(expense.Id, expense.UserId);

            Result result = await commandHandler.Handle(command, default);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
