using System;
using System.Threading.Tasks;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Application.Utilities;
using FluentAssertions;
using Moq;
using Xunit;
using ISqlConnectionFactory = Expensely.Application.Abstractions.Data.ISqlConnectionFactory;

namespace Expensely.Application.UnitTests.Expenses.Queries
{
    public class GetExpensesQueryTests
    {
        private const int Limit = 2;
        private static readonly Guid UserId = Guid.NewGuid();

        [Fact]
        public void Should_construct_properly_with_cursor()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime date = utcNow.Date;
            var cursor = Cursor.Create(
                date.ToString(DateTimeFormats.DatePrecision),
                utcNow.ToString(DateTimeFormats.MillisecondPrecision));

            var query = new GetExpensesQuery(UserId, Limit, cursor, DateTime.UtcNow);

            query.Date.Should().Be(date);
            query.CreatedOnUtc.Should().Be(utcNow);
        }

        [Fact]
        public void Should_create_valid_cache_key_without_cursor()
        {
            var query = new GetExpensesQuery(UserId, Limit, null, DateTime.UtcNow);

            string cacheKey = query.GetCacheKey();

            cacheKey.Should().Be(string.Format(CacheKeys.Expense.ExpensesList, query.UserId, query.Limit, null));
        }

        [Fact]
        public void Should_create_valid_cache_key_with_cursor()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime date = utcNow.Date;
            var cursor = Cursor.Create(
                date.ToString(DateTimeFormats.DatePrecision),
                utcNow.ToString(DateTimeFormats.MillisecondPrecision));
            var query = new GetExpensesQuery(UserId, Limit, cursor, DateTime.UtcNow);

            string cacheKey = query.GetCacheKey();

            cacheKey.Should().Be(string.Format(CacheKeys.Expense.ExpensesList, query.UserId, query.Limit, cursor));
        }

        [Fact]
        public async Task Should_return_empty_response_if_user_id_is_empty()
        {
            var queryHandler = new GetExpensesQueryHandler(new Mock<ISqlConnectionFactory>().Object);
            var query = new GetExpensesQuery(Guid.Empty, Limit, null, DateTime.UtcNow);

            ExpenseListResponse result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().BeEmpty();
        }
    }
}
