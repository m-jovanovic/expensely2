using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Queries.Expenses.GetExpenseById;
using Expensely.Application.Tests.Common;
using Expensely.Contracts.Expenses;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Queries
{
    public class GetExpenseByIdQueryTests : BaseTest
    {
        [Fact]
        public async Task Handle_ShouldReturnNull_GivenEmptyId()
        {
            var query = new GetExpenseByIdQuery(Guid.Empty);

            var queryHandler = new GetExpenseByIdQueryHandler(_dbContext);

            ExpenseDto? result = await queryHandler.Handle(query, default);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_GivenRandomId()
        {
            var query = new GetExpenseByIdQuery(Guid.NewGuid());

            var queryHandler = new GetExpenseByIdQueryHandler(_dbContext);

            ExpenseDto? result = await queryHandler.Handle(query, default);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_ShouldReturnExpenseDto_GivenCorrectId()
        {
            SeedExpenses();

            Expense expense = _dbContext.Set<Expense>().First();

            var query = new GetExpenseByIdQuery(expense.Id);

            var queryHandler = new GetExpenseByIdQueryHandler(_dbContext);

            ExpenseDto? result = await queryHandler.Handle(query, default);

            Assert.NotNull(result);
            Assert.Equal(expense.Id, result!.Id);
            Assert.Equal(expense.Money.Amount, result!.Amount);
            Assert.Equal(expense.Money.Currency.Id, result.CurrencyId);
            Assert.Equal(expense.Money.Currency.Code, result!.CurrencyCode);
            Assert.Equal(expense.CreatedOnUtc, result.CreatedOnUtc);
            Assert.Equal(expense.ModifiedOnUtc, result.ModifiedOnUtc);
            Assert.Equal(expense.Deleted, result.Deleted);
        }

        private void SeedExpenses()
        {
            var expense1 = new Expense(Guid.NewGuid(), Money.Null);

            _dbContext.Add(expense1);

            _dbContext.SaveChanges();
        }
    }
}
