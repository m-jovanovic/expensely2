using System;
using System.Threading.Tasks;
using Expensely.Application.Caching;
using Expensely.Application.Constants;
using Expensely.Application.Expenses.Events.ExpenseDeleted;
using Moq;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Events
{
    public class ExpenseDeleetedEventTests
    {
        [Fact]
        public async Task Handle_should_call_Remove_on_CacheService()
        {
            var cacheServiceMock = new Mock<ICacheService>();
            var handler = new ExpenseDeletedEventHandler(cacheServiceMock.Object);
            var expenseCreated = new ExpenseDeletedEvent(Guid.Empty);

            await handler.Handle(expenseCreated, default);

            cacheServiceMock.Verify(x => x.RemoveValue(It.Is<string>(s => s == CacheKeys.Expenses)));
        }

        [Fact]
        public async Task Handle_should_call_Remove_for_expense_by_id_on_CacheService()
        {
            var cacheServiceMock = new Mock<ICacheService>();
            var handler = new ExpenseDeletedEventHandler(cacheServiceMock.Object);
            var expenseCreated = new ExpenseDeletedEvent(Guid.Empty);

            await handler.Handle(expenseCreated, default);

            cacheServiceMock.Verify(x =>
                x.RemoveValue(It.Is<string>(s => s == string.Format(CacheKeys.ExpenseById, expenseCreated.ExpenseId))));
        }
    }
}
