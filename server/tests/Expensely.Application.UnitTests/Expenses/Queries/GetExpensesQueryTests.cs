﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Application.Utilities;
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
            var queryHandler = new GetExpensesQueryHandler(_dbExecutorMock.Object);
            var query = new GetExpensesQuery(Guid.Empty, Limit, null, DateTime.UtcNow);

            ExpenseListResponse result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().BeEmpty();
        }
        
        [Fact]
        public async Task Should_return_response_with_empty_cursor_if_query_returns_less_expenses_than_limit()
        {
            var response = new[]
            {
                new ExpenseResponse(),
                new ExpenseResponse()
            };
            _dbExecutorMock.Setup(x => x.QueryAsync<ExpenseResponse>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(response);
            var queryHandler = new GetExpensesQueryHandler(_dbExecutorMock.Object);
            var query = new GetExpensesQuery(Guid.NewGuid(), Limit, null, DateTime.UtcNow);

            ExpenseListResponse result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Cursor.Should().BeEmpty();
            result.Items.Should().HaveCount(Limit);
        }

        [Fact]
        public async Task Should_return_response_with_non_empty_cursor_if_query_returns_same_number_of_expenses_as_limit()
        {
            var response = new[]
            {
                new ExpenseResponse(),
                new ExpenseResponse(),
                new ExpenseResponse()
            };
            _dbExecutorMock.Setup(x => x.QueryAsync<ExpenseResponse>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(response);
            var queryHandler = new GetExpensesQueryHandler(_dbExecutorMock.Object);
            var query = new GetExpensesQuery(Guid.NewGuid(), Limit, null, DateTime.UtcNow);

            ExpenseListResponse result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Cursor.Should().NotBeEmpty();
            result.Items.Should().HaveCount(Limit);
        }
    }
}
