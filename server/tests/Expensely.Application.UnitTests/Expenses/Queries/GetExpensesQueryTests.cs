﻿using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Core.Constants;
using Expensely.Application.Core.Utilities;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Transactions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Queries
{
    public class GetExpensesQueryTests
    {
        private const int Limit = 2;
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Mock<IDbExecutor> _dbExecutorMock;

        public GetExpensesQueryTests()
        {
            _dbExecutorMock = new Mock<IDbExecutor>();
        }

        [Fact]
        public void Should_construct_properly_without_cursor()
        {
            DateTime utcNow = DateTime.UtcNow;

            var query = new GetExpensesQuery(UserId, Limit, null, utcNow);

            query.UserId.Should().Be(UserId);
            query.Limit.Should().Be(Limit + 1);
            query.OccurredOn.Should().Be(utcNow.Date);
            query.CreatedOnUtc.Should().Be(utcNow);
        }

        [Fact]
        public void Should_construct_properly_with_cursor()
        {
            DateTime utcNow = DateTime.UtcNow;
            DateTime date = utcNow.Date;
            var cursor = Cursor.Create(
                date.ToString(DateTimeFormats.DatePrecision),
                utcNow.ToString(DateTimeFormats.MillisecondPrecision));

            var query = new GetExpensesQuery(UserId, Limit, cursor, DateTime.UtcNow);

            query.OccurredOn.Should().Be(date);
            query.CreatedOnUtc.Should().Be(utcNow);
        }

        [Fact]
        public void ShouldSetProperLimitIfLimitIsOutOfBounds()
        {
            const int limit = LimitFactory.MaxLimit * 2;
            var query = new GetExpensesQuery(UserId, limit, null, DateTime.UtcNow);

            query.Limit.Should().Be(LimitFactory.MaxLimit + 1);
        }

        [Fact]
        public void Should_create_valid_cache_key_without_cursor()
        {
            var query = new GetExpensesQuery(UserId, Limit, null, DateTime.UtcNow);

            string cacheKey = query.GetCacheKey();

            cacheKey.Should().Be(string.Format(CacheKeys.Expenses.ExpensesList, query.UserId, query.Limit, null));
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

            cacheKey.Should().Be(string.Format(CacheKeys.Expenses.ExpensesList, query.UserId, query.Limit, cursor));
        }

        [Fact]
        public async Task Should_return_empty_response_if_user_id_is_empty()
        {
            var queryHandler = new GetExpensesQueryHandler(_dbExecutorMock.Object);
            var query = new GetExpensesQuery(Guid.Empty, Limit, null, DateTime.UtcNow);

            Result<ExpenseListResponse> result = await queryHandler.Handle(query, default);

            result.Value().Items.Should().BeEmpty();
        }
        
        [Fact]
        public async Task Should_return_response_with_empty_cursor_if_query_returns_less_expenses_than_limit()
        {
            var expenseResponses = new[]
            {
                new ExpenseResponse(Guid.NewGuid(), "Expense", -1.0m, Currency.Usd.Value, DateTime.Now, DateTime.Now),
                new ExpenseResponse(Guid.NewGuid(), "Expense", -1.0m, Currency.Usd.Value, DateTime.Now, DateTime.Now)
            };
            _dbExecutorMock.Setup(x => x.QueryAsync<ExpenseResponse>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(expenseResponses);
            var queryHandler = new GetExpensesQueryHandler(_dbExecutorMock.Object);
            var query = new GetExpensesQuery(Guid.NewGuid(), Limit, null, DateTime.UtcNow);

            Result<ExpenseListResponse> result = await queryHandler.Handle(query, default);

            var response = result.Value();
            response.Should().NotBeNull();
            response.Cursor.Should().BeEmpty();
            response.Items.Should().HaveCount(Limit);
        }

        [Fact]
        public async Task Should_return_response_with_non_empty_cursor_if_query_returns_same_number_of_expenses_as_limit()
        {
            var expenseResponses = new[]
            {
                new ExpenseResponse(Guid.NewGuid(), "Expense", -1.0m, Currency.Usd.Value, DateTime.Now, DateTime.Now),
                new ExpenseResponse(Guid.NewGuid(), "Expense", -1.0m, Currency.Usd.Value, DateTime.Now, DateTime.Now),
                new ExpenseResponse(Guid.NewGuid(), "Expense", -1.0m, Currency.Usd.Value, DateTime.Now, DateTime.Now)
            };
            _dbExecutorMock.Setup(x => x.QueryAsync<ExpenseResponse>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(expenseResponses);
            var queryHandler = new GetExpensesQueryHandler(_dbExecutorMock.Object);
            var query = new GetExpensesQuery(Guid.NewGuid(), Limit, null, DateTime.UtcNow);

            Result<ExpenseListResponse> result = await queryHandler.Handle(query, default);

            var response = result.Value();
            response.Should().NotBeNull();
            response.Cursor.Should().NotBeEmpty();
            response.Items.Should().HaveCount(Limit);
        }
    }
}
