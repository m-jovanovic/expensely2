using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Constants;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Domain.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Queries
{
    public class GetExpenseByIdQueryTests
    {
        [Fact]
        public void Should_create_valid_cache_key()
        {
            var query = new GetExpenseByIdQuery(Guid.NewGuid(), Guid.NewGuid());

            string cacheKey = query.GetCacheKey();

            cacheKey.Should().Be(string.Format(CacheKeys.Expenses.ExpenseById, query.UserId, query.ExpenseId));
        }
        
        [Fact]
        public async Task Should_return_failure_result_given_empty_expense_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(new Mock<IDbContext>().Object);
            var query = new GetExpenseByIdQuery(Guid.Empty, Guid.NewGuid());

            Result<ExpenseResponse> result = await queryHandler.Handle(query, default);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Should_return_failure_result_given_empty_user_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(new Mock<IDbContext>().Object);
            var query = new GetExpenseByIdQuery(Guid.NewGuid(), Guid.Empty);

            Result<ExpenseResponse> result = await queryHandler.Handle(query, default);

            result.IsFailure.Should().BeTrue();
        }
    }
}
