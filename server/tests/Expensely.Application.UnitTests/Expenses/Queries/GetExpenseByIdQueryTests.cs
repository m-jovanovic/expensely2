using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Constants;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Domain.Entities;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using MockQueryable.Moq;
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

            cacheKey.Should().Be(string.Format(CacheKeys.Expense.ExpenseById, query.UserId, query.ExpenseId));
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

        [Fact]
        public async Task Should_return_null_if_expense_with_specified_id_does_not_exist()
        {
            var dbSetMock = new List<Expense>().AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpenseByIdQueryHandler(dbContextMock.Object);
            var query = new GetExpenseByIdQuery(Guid.NewGuid(), Guid.NewGuid());

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_null_if_expense_with_specified_id_exists_but_user_id_is_invalid()
        {
            var expense = ExpenseData.CreateExpense();
            var dbSetMock = new List<Expense>
                {
                    expense
                }
                .AsQueryable()
                .BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpenseByIdQueryHandler(dbContextMock.Object);
            var query = new GetExpenseByIdQuery(expense.Id, Guid.NewGuid());

            ExpenseResponse result = (await queryHandler.Handle(query, default))!;

            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_expense_response_if_expense_with_specified_id_and_user_id_exists()
        {
            var expense = ExpenseData.CreateExpense();
            var dbSetMock = new List<Expense>
                {
                    expense
                }
                .AsQueryable()
                .BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpenseByIdQueryHandler(dbContextMock.Object);
            var query = new GetExpenseByIdQuery(expense.Id, expense.UserId);

            ExpenseResponse result = (await queryHandler.Handle(query, default))!;

            result.Should().NotBeNull();
            result.Id.Should().Be(expense.Id);
            result.Name.Should().Be(expense.Name);
            result.Amount.Should().Be(expense.Money.Amount);
            result.CurrencyId.Should().Be(expense.Money.Currency.Id);
            result.CurrencyCode.Should().Be(expense.Money.Currency.Code);
            result.Date.Should().Be(expense.Date);
            result.CreatedOnUtc.Should().Be(expense.CreatedOnUtc);
        }
    }
}
