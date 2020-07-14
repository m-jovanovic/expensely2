using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Expenses.DeleteExpense;
using Expensely.Application.Events.Expenses.ExpenseDeleted;
using Expensely.Application.Interfaces;
using Expensely.Application.Tests.Common;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Commands
{
    public class DeleteExpenseCommandTests : BaseTest
    {
        [Fact]
        public void Command_should_create_properly()
        {
            var command = new DeleteExpenseCommand(Guid.Empty);

            Assert.NotNull(command);
            Assert.Equal(Guid.Empty, command.ExpenseId);
        }

        [Fact]
        public async Task Handle_should_complete_unsuccessfully_given_empty_expense_id()
        {
            var expenseRepositoryStub = new Mock<IExpenseRepository>();
            var mediatorStub = new Mock<IMediator>();
            var command = new DeleteExpenseCommand(Guid.Empty);
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryStub.Object, mediatorStub.Object);

            Result result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error);
        }

        [Fact]
        public async Task Handle_should_complete_unsuccessfully_given_non_existing_expense_id()
        {
            await SeedExpenses();

            var expenseRepositoryStub = new Mock<IExpenseRepository>();
            var mediatorStub = new Mock<IMediator>();
            var command = new DeleteExpenseCommand(Guid.NewGuid());
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryStub.Object, mediatorStub.Object);

            Result result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error);
        }

        [Fact]
        public async Task Handle_should_call_GetByIdAsync_on_ExpenseRepository()
        {
            await SeedExpenses();

            Guid expenseId = _dbContext.Set<Expense>().First().Id;

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var mediatorStub = new Mock<IMediator>();
            var command = new DeleteExpenseCommand(expenseId);
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, mediatorStub.Object);

            await commandHandler.Handle(command, default);

            expenseRepositoryMock.Verify(x => x.GetByIdAsync(It.Is<Guid>(g => g == expenseId)), Times.Once());
        }

        [Fact]
        public async Task Handle_should_call_Remove_on_ExpenseRepository()
        {
            await SeedExpenses();

            Expense expense = _dbContext.Set<Expense>().First();

            Guid expenseId = expense.Id;

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetByIdAsync(expenseId)).ReturnsAsync(expense);
            var mediatorStub = new Mock<IMediator>();
            var command = new DeleteExpenseCommand(expenseId);
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, mediatorStub.Object);

            await commandHandler.Handle(command, default);

            expenseRepositoryMock.Verify(x => x.Remove(It.Is<Expense>(e => e.Id == expenseId)), Times.Once());
        }

        [Fact]
        public async Task Handle_should_publish_ExpenseDeletedEvent()
        {
            await SeedExpenses();

            Expense expense = _dbContext.Set<Expense>().First();

            Guid expenseId = expense.Id;

            var expenseRepositoryStub = new Mock<IExpenseRepository>();
            expenseRepositoryStub.Setup(e => e.GetByIdAsync(expenseId)).ReturnsAsync(expense);
            var mediatorMock = new Mock<IMediator>();
            var command = new DeleteExpenseCommand(expenseId);
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryStub.Object, mediatorMock.Object);

            await commandHandler.Handle(command, default);

            mediatorMock.Verify(x => x.Publish(It.IsAny<ExpenseDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_should_delete_an_expense()
        {
            await SeedExpenses();

            Expense expense = _dbContext.Set<Expense>().First();

            Guid expenseId = expense.Id;

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expense);
            expenseRepositoryMock.Setup(e => e.Remove(It.IsAny<Expense>())).Callback(() => _dbContext.Remove(expense));
            var mediatorStub = new Mock<IMediator>();
            var command = new DeleteExpenseCommand(expenseId);
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, mediatorStub.Object);

            await commandHandler.Handle(command, default);

            // Calling save changes because this would usually be done by the Unit of Work.
            await _dbContext.SaveChangesAsync();

            Assert.True(_dbContext.Set<Expense>().Count() == 0);
        }

        private async Task SeedExpenses()
        {
            var expense1 = new Expense(Guid.NewGuid(), string.Empty, Money.Null, DateTime.Now);

            _dbContext.Add(expense1);

            await _dbContext.SaveChangesAsync();
        }
    }
}
