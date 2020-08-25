using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Incomes;
using Expensely.Application.Incomes.Queries.GetExpenseById;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;
using static Expensely.Application.IntegrationTests.Common.Testing;

namespace Expensely.Application.IntegrationTests.Incomes.Queries
{
    public class GetIncomeByIdQueryTests
    {
        [Fact]
        public async Task Should_return_null_given_non_existing_income_id()
        {
            var query = new GetIncomeByIdQuery(Guid.NewGuid(), UserId);

            IncomeResponse? result = await SendAsync(query);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_null_response_given_existing_income_id_with_invalid_user_id()
        {
            var income = TransactionData.CreateIncome();
            await AddAsync(income);
            var query = new GetIncomeByIdQuery(income.Id, UserId);

            IncomeResponse? result = await SendAsync(query);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_response_given_existing_income_id_with_valid_user_id()
        {
            var income = TransactionData.CreateIncome(UserId);
            await AddAsync(income);
            var query = new GetIncomeByIdQuery(income.Id, UserId);

            IncomeResponse? result = await SendAsync(query);

            result.Should().NotBeNull();
            result.Id.Should().Be(income.Id);
            result.Name.Should().Be(income.Name);
            result.Amount.Should().Be(income.Money.Amount);
            result.CurrencyCode.Should().Be(income.Money.Currency.Code);
            result.OccurredOn.Should().Be(income.OccurredOn);
            result.CreatedOnUtc.Should().Be(income.CreatedOnUtc);
        }
    }
}
