using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Application.Utilities;
using Expensely.Domain.Entities;
using Expensely.Tests.Common;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Queries
{
    public class GetExpensesQueryTests
    {
        private const int Limit = 2;
        private static readonly Guid UserId = Guid.NewGuid();
        private static readonly DateTime Date = Time.Now().Date;
        private static readonly List<Expense> Expenses = new List<Expense>
        {
            ExpenseData.CreateExpense(UserId, Date),
            ExpenseData.CreateExpense(UserId, Date.AddDays(-1)),
            ExpenseData.CreateExpense(UserId, Date.AddDays(-2))
        };

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
            var dbSetMock = Expenses.AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpensesQueryHandler(dbContextMock.Object);
            var query = new GetExpensesQuery(Guid.Empty, Limit, null, DateTime.UtcNow);

            ExpenseListResponse result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_return_empty_response_if_no_expenses_exist_for_user_id()
        {
            var dbSetMock = new List<Expense>().AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpensesQueryHandler(dbContextMock.Object);
            var query = new GetExpensesQuery(Guid.NewGuid(), Limit, null, DateTime.UtcNow);

            ExpenseListResponse result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Items.Should().NotBeNull();
            result.Items.Should().BeEmpty();
            result.Cursor.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_return_non_empty_response_if_expenses_exist_for_user_id()
        {
            var dbSetMock = Expenses.AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpensesQueryHandler(dbContextMock.Object);
            var query = new GetExpensesQuery(UserId, Limit, null, DateTime.UtcNow);

            ExpenseListResponse result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(Limit);
        }

        [Fact]
        public async Task Should_return_empty_cursor_if_no_more_expenses_exist_for_user_id()
        {
            var dbSetMock = Expenses.AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpensesQueryHandler(dbContextMock.Object);
            var query = new GetExpensesQuery(UserId, Expenses.Count, null, DateTime.UtcNow);

            ExpenseListResponse result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Items.Should().HaveCount(Expenses.Count);
            result.Cursor.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_return_non_empty_cursor_if_more_expenses_exist_for_user_id()
        {
            var dbSetMock = Expenses.AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpensesQueryHandler(dbContextMock.Object);
            var query = new GetExpensesQuery(UserId, Limit, null, DateTime.UtcNow);

            ExpenseListResponse result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Cursor.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Should_return_more_expenses_with_cursor_from_response()
        {
            var dbSetMock = Expenses.AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpensesQueryHandler(dbContextMock.Object);
            var query = new GetExpensesQuery(UserId, Limit, null, DateTime.UtcNow);

            ExpenseListResponse resultWithCursor = await queryHandler.Handle(query, default);
            var queryWithCursor = new GetExpensesQuery(UserId, Limit, resultWithCursor.Cursor, DateTime.UtcNow);
            ExpenseListResponse result = await queryHandler.Handle(queryWithCursor, default);

            result.Should().NotBeNull();
            result.Items.Should().NotBeEmpty();
            result.Cursor.Should().BeEmpty();
        }
    }
}
