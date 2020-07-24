using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Application.Tests.Common;
using Expensely.Domain.Entities;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Queries
{
    public class GetExpensesQueryTests : BaseTest
    {
        [Fact]
        public async Task Should_return_empty_collection_if_no_expenses_exist()
        {
            var queryHandler = new GetExpensesQueryHandler(DbContext);
            var query = new GetExpensesQuery();

            IReadOnlyCollection<ExpenseResponse> result = await queryHandler.Handle(query, default);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_return_correct_number_of_expenses_if_expenses_exist()
        {
            await SeedExpenses();

            var queryHandler = new GetExpensesQueryHandler(DbContext);
            var query = new GetExpensesQuery();

            IReadOnlyCollection<ExpenseResponse> result = await queryHandler.Handle(query, default);

            Assert.NotNull(result);
            Assert.True(result.Count == DbContext.Set<Expense>().Count());
        }

        private async Task SeedExpenses()
        {
            var expense1 = new Expense(Guid.NewGuid(), string.Empty, default, DateTime.Now);
            var expense2 = new Expense(Guid.NewGuid(), string.Empty, default, DateTime.Now);
            var expense3 = new Expense(Guid.NewGuid(), string.Empty, default, DateTime.Now);

            DbContext.Add(expense1);
            DbContext.Add(expense2);
            DbContext.Add(expense3);

            await DbContext.SaveChangesAsync();
        }
    }
}
