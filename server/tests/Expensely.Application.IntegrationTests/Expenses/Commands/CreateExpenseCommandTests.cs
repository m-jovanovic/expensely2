using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.IntegrationTests.Common;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.IntegrationTests.Expenses.Commands
{
    public class CreateExpenseCommandTests : DbContextTest
    {
        private const string Name = "Expense";
        private const decimal Amount = 1.0m;
        private const int CurrencyId = 1;
        private static readonly DateTime Date = DateTime.Now;

        [Fact]
        public async Task Handle_should_create_an_expense()
        {
            var commandHandler = new CreateExpenseCommandHandler(new ExpenseRepository(DbContext), new Mock<IMediator>().Object);
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, Date);

            await commandHandler.Handle(command, default);

            await DbContext.SaveChangesAsync();

            DbContext.Set<Expense>().Count().Should().Be(1);
        }

        [Fact]
        public async Task Handle_should_return_entity_created_response_with_id_of_created_expense()
        {
            var commandHandler = new CreateExpenseCommandHandler(new ExpenseRepository(DbContext), new Mock<IMediator>().Object);
            var command = new CreateExpenseCommand(Name, Amount, CurrencyId, Date);

            Result<EntityCreatedResponse> result = await commandHandler.Handle(command, default);

            await DbContext.SaveChangesAsync();

            result.Should().NotBeNull();
            result.Invoking(r => r.Value()).Should().NotThrow();
            EntityCreatedResponse entityCreatedResponse = result.Value();
            entityCreatedResponse.Should().NotBeNull();
            entityCreatedResponse.Id.Should().NotBeEmpty();
            entityCreatedResponse.Id.Should().Be(DbContext.Set<Expense>().Single().Id);
        }
    }
}
