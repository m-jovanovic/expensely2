using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Constants;
using Expensely.Application.Expenses.Queries.GetExpenseById;
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
        public async Task Should_return_null_given_empty_expense_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(new Mock<IDbContext>().Object);
            var query = new GetExpenseByIdQuery(Guid.Empty, Guid.NewGuid());

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_null_given_empty_user_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(new Mock<IDbContext>().Object);
            var query = new GetExpenseByIdQuery(Guid.NewGuid(), Guid.Empty);

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            result.Should().BeNull();
        }
    }
}
