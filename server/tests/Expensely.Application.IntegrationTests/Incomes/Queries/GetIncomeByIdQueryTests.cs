using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Incomes;
using Expensely.Application.Incomes.Queries.GetExpenseById;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Incomes.Queries
{
    public class GetIncomeByIdQueryTests
    {
        [Fact]
        public async Task Should_return_failure_result_given_non_existing_income_id()
        {
            var query = new GetIncomeByIdQuery(Guid.NewGuid(), UserId);

            Result<IncomeResponse> result = await SendAsync(query);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Should_return_failure_result_response_given_existing_income_id_with_invalid_user_id()
        {
            var income = TransactionData.CreateIncome();
            await AddAsync(income);
            var query = new GetIncomeByIdQuery(income.Id, UserId);

            Result<IncomeResponse> result = await SendAsync(query);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Should_return_response_given_existing_income_id_with_valid_user_id()
        {
            var income = TransactionData.CreateIncome(UserId);
            await AddAsync(income);
            var query = new GetIncomeByIdQuery(income.Id, UserId);

            Result<IncomeResponse> result = await SendAsync(query);

            IncomeResponse response = result.Value();
            response.Id.Should().Be(income.Id);
            response.Name.Should().Be(income.Name);
            response.Amount.Should().Be(income.Money.Amount);
            response.CurrencyCode.Should().Be(income.Money.Currency.Code);
            response.OccurredOn.Should().Be(income.OccurredOn);
            response.CreatedOnUtc.Should().Be(income.CreatedOnUtc);
        }
    }
}
