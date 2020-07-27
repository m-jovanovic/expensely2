﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Application.IntegrationTests.Common;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Application.IntegrationTests.Expenses.Queries
{
    public class GetExpenseByIdQueryTests : BaseTest
    {
        private const string Name = "Expense";
        private static readonly Money Money = new Money(decimal.Zero, Currency.Usd);

        [Fact]
        public async Task Handle_should_return_null_given_empty_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(DbContext);
            var query = new GetExpenseByIdQuery(Guid.Empty);

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_should_return_null_given_non_existing_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(DbContext);
            var query = new GetExpenseByIdQuery(Guid.NewGuid());

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_should_return_ExpenseDto_given_existing_id()
        {
            await SeedExpenses();

            var queryHandler = new GetExpenseByIdQueryHandler(DbContext);
            Expense expense = DbContext.Set<Expense>().First();
            var query = new GetExpenseByIdQuery(expense.Id);

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            Assert.NotNull(result);
            Assert.Equal(expense.Id, result!.Id);
            Assert.Equal(expense.Name, result!.Name);
            Assert.Equal(expense.Money.Amount, result!.Amount);
            Assert.Equal(expense.Money.Currency.Id, result.CurrencyId);
            Assert.Equal(expense.Money.Currency.Code, result!.CurrencyCode);
            Assert.Equal(expense.Date, result!.Date);
            Assert.Equal(expense.CreatedOnUtc, result.CreatedOnUtc);
            Assert.Equal(expense.ModifiedOnUtc, result.ModifiedOnUtc);
            Assert.Equal(expense.Deleted, result.Deleted);
        }

        private async Task SeedExpenses()
        {
            var expense = new Expense(Guid.NewGuid(), Name, Money, DateTime.Now);

            DbContext.Add(expense);

            await DbContext.SaveChangesAsync();
        }
    }
}
