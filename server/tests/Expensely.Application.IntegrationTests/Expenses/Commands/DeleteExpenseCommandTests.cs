using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.IntegrationTests.Common;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Expensely.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace Expensely.Application.IntegrationTests.Expenses.Commands
{
    public class DeleteExpenseCommandTests : DbContextTest
    {
        [Fact]
        public async Task Handle_should_fail_given_non_existing_expense_id()
        {
            await SeedExpenses();
            var commandHandler = new DeleteExpenseCommandHandler(new ExpenseRepository(DbContext), new Mock<IMediator>().Object);
            var command = new DeleteExpenseCommand(Guid.NewGuid());

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.General.EntityNotFound);
        }

        [Fact]
        public async Task Handle_should_delete_an_expense()
        {
            await SeedExpenses();
            var commandHandler = new DeleteExpenseCommandHandler(new ExpenseRepository(DbContext), new Mock<IMediator>().Object);
            Guid expenseId = DbContext.Set<Expense>().First().Id;
            var command = new DeleteExpenseCommand(expenseId);

            await commandHandler.Handle(command, default);

            await DbContext.SaveChangesAsync();

            DbContext.Set<Expense>().SingleOrDefault(x => x.Id == expenseId).Should().BeNull();
        }

        private async Task SeedExpenses()
        {
            var expense = new Expense(Guid.NewGuid(), "Expense", new Money(decimal.Zero, Currency.Usd), DateTime.Now);

            DbContext.Add(expense);

            await DbContext.SaveChangesAsync();
        }
    }
}
