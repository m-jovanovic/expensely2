using System;
using System.Threading.Tasks;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Tests.Common;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Expenses.Queries
{
    public class GetExpensesQueryTests
    {
        private const int Limit = 1;
        
        [Fact]
        public async Task Should_return_non_empty_response_if_expenses_exist_for_user_id()
        {
            await SeedExpenses();
            var query = new GetExpensesQuery(UserId, Limit, null);

            var result = await SendAsync(query);

            result.Should().NotBeNull();
            result.Items.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Should_return_empty_cursor_if_no_more_expenses_exist_for_user_id()
        {
            await SeedExpenses();
            var query = new GetExpensesQuery(UserId, 100, null);

            var result = await SendAsync(query);

            result.Should().NotBeNull();
            result.Cursor.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_return_more_expenses_with_cursor_from_response()
        {
            await SeedExpenses();
            var query = new GetExpensesQuery(UserId, Limit, null);

            var resultWithCursor = await SendAsync(query);
            var queryWithCursor = new GetExpensesQuery(UserId, Limit, resultWithCursor.Cursor);
            var result = await SendAsync(queryWithCursor);

            result.Should().NotBeNull();
            result.Items.Should().NotBeEmpty();
        }

        private static async Task SeedExpenses()
        {
            DateTime date = Time.Now().Date;

            await AddAsync(ExpenseData.CreateExpense(UserId, date));
            
            await AddAsync(ExpenseData.CreateExpense(UserId, date.AddDays(-1)));
            
            await AddAsync(ExpenseData.CreateExpense(UserId, date.AddDays(-2)));
        }
    }
}
