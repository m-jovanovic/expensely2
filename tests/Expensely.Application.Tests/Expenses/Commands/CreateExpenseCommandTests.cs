using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Commands.Expenses.CreateExpense;
using Expensely.Application.Events.Expenses.ExpenseCreated;
using Expensely.Application.Interfaces;
using Expensely.Application.Tests.Common;
using Expensely.Common.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Expensely.Persistence.Repositories;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Commands
{
    public class CreateExpenseCommandTests : BaseTest
    {
        private static readonly decimal Amount = 0.0m;
        private static readonly int CurrencyId = Currency.Usd.Id;

        [Fact]
        public void Command_Should_CreateProperly()
        {
            var command = new CreateExpenseCommand(Amount, CurrencyId);

            Assert.NotNull(command);
            Assert.Equal(Amount, command.Amount);
            Assert.Equal(CurrencyId, command.CurrencyId);
        }

        [Fact]
        public async Task Handle_Should_CompleteSuccessfully()
        {
            var expenseRepositoryStub = new Mock<IExpenseRepository>();
            var mediatorStub = new Mock<IMediator>();
            var command = new CreateExpenseCommand(Amount, CurrencyId);
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryStub.Object, mediatorStub.Object);

            Result result = await commandHandler.Handle(command, default);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_Should_CallInsert_OnExpenseRepository()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var mediatorStub = new Mock<IMediator>();
            var command = new CreateExpenseCommand(Amount, CurrencyId);
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryMock.Object, mediatorStub.Object);

            await commandHandler.Handle(command, default);

            expenseRepositoryMock.Verify(x => x.Insert(It.IsAny<Expense>()), Times.Once());
        }

        [Fact]
        public async Task Handle_Should_Publish_ExpenseCreatedEvent()
        {
            var expenseRepositorystub = new Mock<IExpenseRepository>();
            var mediatorMock = new Mock<IMediator>();
            var command = new CreateExpenseCommand(Amount, CurrencyId);
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositorystub.Object, mediatorMock.Object);

            await commandHandler.Handle(command, default);

            mediatorMock.Verify(x => x.Publish(It.IsAny<ExpenseCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_Should_CreateAnExpense()
        {
            var expenseRepositoryStub = new ExpenseRepository(_dbContext);
            var mediatorStub = new Mock<IMediator>();
            var command = new CreateExpenseCommand(Amount, CurrencyId);
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryStub, mediatorStub.Object);

            await commandHandler.Handle(command, default);

            // Calling save changes because this would usually be done by the Unit of Work.
            await _dbContext.SaveChangesAsync();

            Assert.True(_dbContext.Set<Expense>().Count() == 1);
        }
    }
}
