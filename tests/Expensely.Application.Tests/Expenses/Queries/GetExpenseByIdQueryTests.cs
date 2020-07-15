using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Application.Tests.Common;
using Expensely.Common.Contracts.Expenses;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Queries
{
    public class GetExpenseByIdQueryTests : BaseTest
    {
        [Fact]
        public async Task Handle_should_return_null_given_empty_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(_dbContext);
            var query = new GetExpenseByIdQuery(Guid.Empty);

            ExpenseDto? result = await queryHandler.Handle(query, default);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_should_return_null_given_non_existing_id()
        {
            var queryHandler = new GetExpenseByIdQueryHandler(_dbContext);
            var query = new GetExpenseByIdQuery(Guid.NewGuid());

            ExpenseDto? result = await queryHandler.Handle(query, default);

            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_should_return_ExpenseDto_given_existing_id()
        {
            await SeedExpenses();

            var queryHandler = new GetExpenseByIdQueryHandler(_dbContext);
            Expense expense = _dbContext.Set<Expense>().First();
            var query = new GetExpenseByIdQuery(expense.Id);

            ExpenseDto? result = await queryHandler.Handle(query, default);

            Assert.NotNull(result);
            Assert.Equal(expense.Id, result!.Id);
            Assert.Equal(expense.Name, result!.Name);
            Assert.Equal(expense.Money.Amount, result!.Amount);
            Assert.Equal(expense.Money.Currency.Id, result.CurrencyId);
            Assert.Equal(expense.Money.Currency.Code, result!.CurrencyCode);
            Assert.Equal(expense.Date, result!.Date);
            Assert.Equal(expense.CreatedOnUtc, result.CreatedOnUtc);
            Assert.Equal(expense.ModifiedOnUtc, result.ModifiedOnUtc);
            Assert.Equal(expense.Deleted, result.Deleted);
        }

        private async Task SeedExpenses()
        {
            var expense1 = new Expense(Guid.NewGuid(), string.Empty, Money.Null, DateTime.Now);

            _dbContext.Add(expense1);

            await _dbContext.SaveChangesAsync();
        }
    }
}
