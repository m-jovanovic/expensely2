using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Tests.Common.Data;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Expenses.Queries
{
    public class GetExpensesQueryTests
    {
        [Fact]
        public async Task Should_return_non_empty_collection_of_expense_responses_if_expenses_exist()
        {
            await SeedExpenses();
            var query = new GetExpensesQuery();

            IReadOnlyCollection<ExpenseResponse> result = await SendAsync(query);

            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().NotContainNulls();
        }

        private static async Task SeedExpenses()
        {
            await AddAsync(ExpenseData.Expense);
            
            await AddAsync(ExpenseData.Expense);
            
            await AddAsync(ExpenseData.Expense);
        }
    }
}
