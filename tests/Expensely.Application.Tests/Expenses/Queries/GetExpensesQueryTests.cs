using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Application.Queries.Expenses.GetExpenses;
using Expensely.Application.Tests.Common;
using Expensely.Common.Contracts.Expenses;
using Expensely.Domain.Entities;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Queries
{
    public class GetExpensesQueryTests : BaseTest
    {
        [Fact]
        public async Task Should_return_empty_collection_if_no_expenses_exist()
        {
            var query = new GetExpensesQuery();

            var queryHandler = new GetExpensesQueryHandler(_dbContext);

            IReadOnlyCollection<ExpenseDto> result = await queryHandler.Handle(query, default);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_return_correct_number_of_expenses_if_expenses_exist()
        {
            await SeedExpenses();

            var query = new GetExpensesQuery();

            var queryHandler = new GetExpensesQueryHandler(_dbContext);

            IReadOnlyCollection<ExpenseDto> result = await queryHandler.Handle(query, default);

            Assert.NotNull(result);
            Assert.True(result.Count == 3);
        }

        private async Task SeedExpenses()
        {
            var expense1 = new Expense(Guid.NewGuid(), string.Empty, default, DateTime.Now);
            var expense2 = new Expense(Guid.NewGuid(), string.Empty, default, DateTime.Now);
            var expense3 = new Expense(Guid.NewGuid(), string.Empty, default, DateTime.Now);

            _dbContext.Add(expense1);
            _dbContext.Add(expense2);
            _dbContext.Add(expense3);

            await _dbContext.SaveChangesAsync();
        }
    }
}
