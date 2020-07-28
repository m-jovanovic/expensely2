using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.Expenses.Events.ExpenseDeleted;
using Expensely.Application.IntegrationTests.Common;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.IntegrationTests.Expenses.Commands
{
    public class DeleteExpenseCommandTests : BaseTest
    {
        private const string Name = "Expense";
        private static readonly Money Money = new Money(decimal.Zero, Currency.Usd);

        [Fact]
        public async Task Handle_should_complete_unsuccessfully_given_non_existing_expense_id()
        {
            await SeedExpenses();

            var expenseRepositoryStub = new Mock<IExpenseRepository>();
            var mediatorStub = new Mock<IMediator>();
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryStub.Object, mediatorStub.Object);
            var command = new DeleteExpenseCommand(Guid.NewGuid());

            Result result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.Equal(Errors.General.EntityNotFound, result.Error);
        }

        [Fact]
        public async Task Handle_should_call_GetByIdAsync_on_ExpenseRepository()
        {
            await SeedExpenses();

            Guid expenseId = DbContext.Set<Expense>().First().Id;

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var mediatorStub = new Mock<IMediator>();
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, mediatorStub.Object);
            var command = new DeleteExpenseCommand(expenseId);

            await commandHandler.Handle(command, default);

            expenseRepositoryMock.Verify(x => x.GetByIdAsync(It.Is<Guid>(g => g == expenseId)), Times.Once());
        }

        [Fact]
        public async Task Handle_should_call_Remove_on_ExpenseRepository()
        {
            await SeedExpenses();

            Expense expense = DbContext.Set<Expense>().First();

            Guid expenseId = expense.Id;

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetByIdAsync(expenseId)).ReturnsAsync(expense);
            var mediatorStub = new Mock<IMediator>();
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, mediatorStub.Object);
            var command = new DeleteExpenseCommand(expenseId);

            await commandHandler.Handle(command, default);

            expenseRepositoryMock.Verify(x => x.Remove(It.Is<Expense>(e => e.Id == expenseId)), Times.Once());
        }

        [Fact]
        public async Task Handle_should_publish_ExpenseDeletedEvent()
        {
            await SeedExpenses();

            Expense expense = DbContext.Set<Expense>().First();

            Guid expenseId = expense.Id;

            var expenseRepositoryStub = new Mock<IExpenseRepository>();
            expenseRepositoryStub.Setup(e => e.GetByIdAsync(expenseId)).ReturnsAsync(expense);
            var mediatorMock = new Mock<IMediator>();
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryStub.Object, mediatorMock.Object);
            var command = new DeleteExpenseCommand(expenseId);

            await commandHandler.Handle(command, default);

            mediatorMock.Verify(x => x.Publish(It.IsAny<ExpenseDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_should_delete_an_expense()
        {
            await SeedExpenses();

            Expense expense = DbContext.Set<Expense>().First();

            Guid expenseId = expense.Id;

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expense);
            expenseRepositoryMock.Setup(e => e.Remove(It.IsAny<Expense>())).Callback(() => DbContext.Remove(expense));
            var mediatorStub = new Mock<IMediator>();
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, mediatorStub.Object);
            var command = new DeleteExpenseCommand(expenseId);

            await commandHandler.Handle(command, default);

            // Calling save changes because this would usually be done by the Unit of Work.
            await DbContext.SaveChangesAsync();

            Assert.True(DbContext.Set<Expense>().Count() == 0);
        }

        private async Task SeedExpenses()
        {
            var expense = new Expense(Guid.NewGuid(), Name, Money, DateTime.Now);

            DbContext.Add(expense);

            await DbContext.SaveChangesAsync();
        }
    }
}
