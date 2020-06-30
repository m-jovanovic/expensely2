using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Application.Queries.Expenses.GetExpenses;
using Expensely.Application.Tests.Common;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Entities;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Queries
{
    public class GetExpensesQueryTests : BaseTest
    {
        [Fact]
        public async Task Should_ReturnEmptyCollection_IfNoExpensesExist()
        {
            var query = new GetExpensesQuery();

            var queryHandler = new GetExpensesQueryHandler(_dbContext);

            IReadOnlyCollection<ExpenseDto> result = await queryHandler.Handle(query, default);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Should_ReturnCorrectNumberOfExpenses_IfExpensesExist()
        {
            SeedExpenses();

            var query = new GetExpensesQuery();

            var queryHandler = new GetExpensesQueryHandler(_dbContext);

            IReadOnlyCollection<ExpenseDto> result = await queryHandler.Handle(query, default);

            Assert.NotNull(result);
            Assert.True(result.Count == 3);
        }

        private void SeedExpenses()
        {
            var expense1 = new Expense(Guid.NewGuid(), default);
            var expense2 = new Expense(Guid.NewGuid(), default);
            var expense3 = new Expense(Guid.NewGuid(), default);

            _dbContext.Add(expense1);
            _dbContext.Add(expense2);
            _dbContext.Add(expense3);

            _dbContext.SaveChanges();
        }
    }
}
