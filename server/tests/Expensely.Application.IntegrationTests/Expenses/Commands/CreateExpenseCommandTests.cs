using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.IntegrationTests.Common;
using Expensely.Domain.Entities;
using Expensely.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.IntegrationTests.Expenses.Commands
{
    public class CreateExpenseCommandTests : BaseTest
    {
        private const string Name = "Expense";
        private const decimal Amount = 1.0m;
        private const int CurrencyId = 1;

        [Fact]
        public async Task Handle_should_create_an_expense()
        {
            var commandHandler = new CreateExpenseCommandHandler(new ExpenseRepository(DbContext), new Mock<IMediator>().Object);
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, DateTime.Now);

            await commandHandler.Handle(command, default);

            await DbContext.SaveChangesAsync();

            DbContext.Set<Expense>().Count().Should().Be(1);
        }
    }
}
