using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.IntegrationTests.Common;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using FluentAssertions;
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

            var commandHandler = new DeleteExpenseCommandHandler(
                new Mock<IExpenseRepository>().Object,
                new Mock<IMediator>().Object);
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

            Expense expense = DbContext.Set<Expense>().First();
            Guid expenseId = expense.Id;

            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            expenseRepositoryMock.Setup(e => e.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(expense);
            expenseRepositoryMock.Setup(e => e.Remove(It.IsAny<Expense>())).Callback(() => DbContext.Remove(expense));
            var commandHandler = new DeleteExpenseCommandHandler(expenseRepositoryMock.Object, new Mock<IMediator>().Object);
            var command = new DeleteExpenseCommand(expenseId);

            await commandHandler.Handle(command, default);

            await DbContext.SaveChangesAsync();

            DbContext.Set<Expense>().Count().Should().Be(0);
        }

        private async Task SeedExpenses()
        {
            var expense = new Expense(Guid.NewGuid(), Name, Money, DateTime.Now);

            DbContext.Add(expense);

            await DbContext.SaveChangesAsync();
        }
    }
}
