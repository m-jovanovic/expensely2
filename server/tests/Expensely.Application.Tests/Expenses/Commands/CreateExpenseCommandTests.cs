using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.Expenses.Events.ExpenseCreated;
using Expensely.Application.Tests.Common;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Expensely.Infrastructure.Persistence.Repositories;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Commands
{
    public class CreateExpenseCommandTests : BaseTest
    {
        private static readonly decimal Amount = 0.0m;
        private static readonly int CurrencyId = Currency.Usd.Id;
        private static readonly string Name = "Expense";

        [Fact]
        public void Command_should_create_properly()
        {
            DateTime now = DateTime.Now;

            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, now);

            Assert.NotNull(command);
            Assert.Equal(Name, command.Name);
            Assert.Equal(Amount, command.Amount);
            Assert.Equal(CurrencyId, command.CurrencyId);
            Assert.Equal(now, command.Date);
        }

        [Fact]
        public async Task Handle_should_complete_successfully()
        {
            var expenseRepositoryStub = new Mock<IExpenseRepository>();
            var mediatorStub = new Mock<IMediator>();
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryStub.Object, mediatorStub.Object);
            DateTime now = DateTime.Now;
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, now);

            Result result = await commandHandler.Handle(command, default);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_should_call_Insert_on_ExpenseRepository()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var mediatorStub = new Mock<IMediator>();
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryMock.Object, mediatorStub.Object);
            DateTime now = DateTime.Now;
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, now);

            await commandHandler.Handle(command, default);

            expenseRepositoryMock.Verify(x => x.Insert(It.IsAny<Expense>()), Times.Once());
        }

        [Fact]
        public async Task Handle_should_publish_ExpenseCreatedEvent()
        {
            var expenseRepositorystub = new Mock<IExpenseRepository>();
            var mediatorMock = new Mock<IMediator>();
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositorystub.Object, mediatorMock.Object);
            DateTime now = DateTime.Now;
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, now);

            await commandHandler.Handle(command, default);

            mediatorMock.Verify(x => x.Publish(It.IsAny<ExpenseCreatedEvent>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task Handle_should_create_an_expense()
        {
            var expenseRepositoryStub = new ExpenseRepository(_dbContext);
            var mediatorStub = new Mock<IMediator>();
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryStub, mediatorStub.Object);
            DateTime now = DateTime.Now;
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, now);

            await commandHandler.Handle(command, default);

            // Calling save changes because this would usually be done by the Unit of Work.
            await _dbContext.SaveChangesAsync();

            Assert.True(_dbContext.Set<Expense>().Count() == 1);
        }
    }
}
