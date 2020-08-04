using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Data;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain.Entities;
using Expensely.Tests.Common.Data;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Queries
{
    public class GetExpensesQueryTests
    {
        [Fact]
        public async Task Should_return_empty_collection_if_no_expenses_exist()
        {
            var dbSetMock = new List<Expense>().AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpensesQueryHandler(dbContextMock.Object);
            var query = new GetExpensesQuery();

            IReadOnlyCollection<ExpenseResponse> result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_return_non_empty_collection_if_expenses_exist()
        {
            var expenses = new List<Expense>
            {
                ExpenseData.Expense,
                ExpenseData.Expense,
                ExpenseData.Expense
            };
            var dbSetMock = expenses.AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpensesQueryHandler(dbContextMock.Object);
            var query = new GetExpensesQuery();

            IReadOnlyCollection<ExpenseResponse> result = await queryHandler.Handle(query, default);

            result.Should().NotBeNull();
            result.Should().HaveCount(expenses.Count);
        }
    }
}
