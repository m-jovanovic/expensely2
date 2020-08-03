using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
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
            var expense1 = new Expense(Guid.NewGuid(), string.Empty, new Money(1.0m, Currency.Usd), DateTime.Now);
            var expense2 = new Expense(Guid.NewGuid(), string.Empty, new Money(1.0m, Currency.Usd), DateTime.Now);
            var expense3 = new Expense(Guid.NewGuid(), string.Empty, new Money(1.0m, Currency.Usd), DateTime.Now);

            await AddAsync(expense1);
            await AddAsync(expense2);
            await AddAsync(expense3);
        }
    }
}
