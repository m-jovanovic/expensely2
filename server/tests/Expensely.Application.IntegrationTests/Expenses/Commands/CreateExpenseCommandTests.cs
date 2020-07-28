using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.IntegrationTests.Common;
using Expensely.Domain.Entities;
using Expensely.Infrastructure.Persistence.Repositories;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.IntegrationTests.Expenses.Commands
{
    public class CreateExpenseCommandTests : BaseTest
    {
        private const string Name = "Expense";
        private const decimal Amount = 0.0m;
        private const int CurrencyId = 1;

        [Fact]
        public async Task Handle_should_create_an_expense()
        {
            var expenseRepositoryStub = new ExpenseRepository(DbContext);
            var mediatorStub = new Mock<IMediator>();
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryStub, mediatorStub.Object);
            DateTime now = DateTime.Now;
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, now);

            await commandHandler.Handle(command, default);

            // Calling save changes because this would usually be done by the Unit of Work.
            await DbContext.SaveChangesAsync();

            Assert.True(DbContext.Set<Expense>().Count() == 1);
        }
    }
}
