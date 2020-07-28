using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Queries
{
    public class GetExpenseByIdQueryTests
    {
        [Fact]
        public async Task Should_return_null_given_empty_expense_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(new Mock<IDbContext>().Object);
            var query = new GetExpenseByIdQuery(Guid.Empty);

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_null_if_expense_with_specified_id_does_not_exist()
        {
            var dbSetMock = new List<Expense>().AsQueryable().BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpenseByIdQueryHandler(dbContextMock.Object);
            var query = new GetExpenseByIdQuery(Guid.NewGuid());

            ExpenseResponse? result = await queryHandler.Handle(query, default);

            Assert.Null(result);
        }

        [Fact]
        public async Task Should_return_expense_response_if_expense_with_specified_id_exists()
        {
            var expense = CreateExpense();
            var dbSetMock = new List<Expense>
                {
                    expense
                }
                .AsQueryable()
                .BuildMockDbSet();
            var dbContextMock = new Mock<IDbContext>();
            dbContextMock.Setup(x => x.Set<Expense>()).Returns(dbSetMock.Object);
            var queryHandler = new GetExpenseByIdQueryHandler(dbContextMock.Object);
            var query = new GetExpenseByIdQuery(expense.Id);

            ExpenseResponse result = (await queryHandler.Handle(query, default))!;

            Assert.NotNull(result);
            Assert.Equal(expense.Id, result.Id);
            Assert.Equal(expense.Name, result.Name);
            Assert.Equal(expense.Money.Amount, result.Amount);
            Assert.Equal(expense.Money.Currency.Id, result.CurrencyId);
            Assert.Equal(expense.Money.Currency.Code, result.CurrencyCode);
            Assert.Equal(expense.Date, result.Date);
            Assert.Equal(expense.CreatedOnUtc, result.CreatedOnUtc);
            Assert.Equal(expense.ModifiedOnUtc, result.ModifiedOnUtc);
            Assert.Equal(expense.Deleted, result.Deleted);
        }

        private static Expense CreateExpense()
            => new Expense(Guid.NewGuid(), "Expense name", new Money(1.0m, Currency.Usd), DateTime.Now);
    }
}
