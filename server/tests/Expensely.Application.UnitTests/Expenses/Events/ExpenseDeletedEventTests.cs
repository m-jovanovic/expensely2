using System;
using System.Threading.Tasks;
using Expensely.Application.Caching;
using Expensely.Application.Constants;
using Expensely.Application.Expenses.Events.ExpenseDeleted;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Events
{
    public class ExpenseDeleetedEventTests
    {
        [Fact]
        public async Task Handle_should_call_remove_with_expenses_key_on_cache_service()
        {
            var cacheServiceMock = new Mock<ICacheService>();
            var eventHandler = new ExpenseDeletedEventHandler(cacheServiceMock.Object);
            var @event = new ExpenseDeletedEvent(Guid.Empty);

            await eventHandler.Handle(@event, default);

            cacheServiceMock.Verify(x => x.RemoveValue(It.Is<string>(s => s == CacheKeys.Expenses)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_call_remove_with_expense_by_id_key_on_cache_service()
        {
            var cacheServiceMock = new Mock<ICacheService>();
            var eventHandler = new ExpenseDeletedEventHandler(cacheServiceMock.Object);
            var @event = new ExpenseDeletedEvent(Guid.Empty);
            string cacheKey = string.Format(CacheKeys.ExpenseById, @event.ExpenseId);

            await eventHandler.Handle(@event, default);

            cacheServiceMock.Verify(x => x.RemoveValue(It.Is<string>(s => s == cacheKey)), Times.Once);
        }
    }
}
