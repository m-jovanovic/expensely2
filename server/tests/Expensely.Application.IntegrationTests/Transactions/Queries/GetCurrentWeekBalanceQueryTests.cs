using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Transactions.Queries.GetCurrentWeekBalance;
using Expensely.Domain.Core;
using Expensely.Domain.Transactions;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Transactions.Queries
{
    public class GetCurrentWeekBalanceQueryTests
    {
        [Fact]
        public async Task Should_return_response_if_there_are_no_transaction_for_user_id()
        {
            var query = new GetCurrentWeekBalanceQuery(UserId, TransactionData.Currency.Value, DateTime.UtcNow);

            Result<BalanceResponse> result = await SendAsync(query);

            var response = result.Value();
            response.Should().NotBeNull();
            response.Amount.Should().Be(decimal.Zero);
            response.Balance.Should().Be(TransactionData.Currency.Format(decimal.Zero));
        }

        [Fact]
        public async Task Should_return_response_if_there_are_transactions_for_user_id()
        {
            var expense = TransactionData.CreateExpense(UserId);
            var income = TransactionData.CreateIncome(UserId);
            await AddAsync(expense);
            await AddAsync(income);
            var query = new GetCurrentWeekBalanceQuery(UserId, TransactionData.Currency.Value, DateTime.UtcNow);

            Result<BalanceResponse> result = await SendAsync(query);

            var response = result.Value();
            response.Should().NotBeNull();
            Money sum = expense.Money + income.Money;
            response.Amount.Should().Be(sum.Amount);
            response.Balance .Should().Be(TransactionData.Currency.Format(sum.Amount));
        }
    }
}
