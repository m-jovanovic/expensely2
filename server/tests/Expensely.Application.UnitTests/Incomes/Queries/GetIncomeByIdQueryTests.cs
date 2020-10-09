using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Incomes;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Constants;
using Expensely.Application.Incomes.Queries.GetExpenseById;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using FluentAssertions;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Incomes.Queries
{
    public class GetIncomeByIdQueryTests
    {
        [Fact]
        public void Should_create_valid_cache_key()
        {
            var query = new GetIncomeByIdQuery(Guid.NewGuid(), Guid.NewGuid());

            string cacheKey = query.GetCacheKey();

            cacheKey.Should().Be(string.Format(CacheKeys.Incomes.IncomeById, query.UserId, query.IncomeId));
        }
        
        [Fact]
        public async Task Should_return_failure_result_given_empty_income_id()
        {
            var queryHandler = new GetIncomeByIdQueryHandler(new Mock<IDbContext>().Object);
            var query = new GetIncomeByIdQuery(Guid.Empty, Guid.NewGuid());

            Result<IncomeResponse> result = await queryHandler.Handle(query, default);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Should_return_failure_result_given_empty_user_id()
        {
            var queryHandler = new GetIncomeByIdQueryHandler(new Mock<IDbContext>().Object);
            var query = new GetIncomeByIdQuery(Guid.NewGuid(), Guid.Empty);

            Result<IncomeResponse> result = await queryHandler.Handle(query, default);

            result.IsFailure.Should().BeTrue();
        }
    }
}
