using System;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Caching;
using Expensely.Application.Constants;
using Expensely.Application.Expenses.Events.ExpenseCreated;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Events
{
    public class ExpenseCreatedEventTests
    {
        [Fact]
        public async Task Handle_should_call_remove_on_cache_service()
        {
            var cacheServiceMock = new Mock<ICacheService>();
            var eventHandler = new ExpenseCreatedEventHandler(cacheServiceMock.Object);
            var @event = new ExpenseCreatedEvent(Guid.Empty);

            await eventHandler.Handle(@event, default);

            cacheServiceMock.Verify(x => x.RemoveValue(It.Is<string>(s => s == CacheKeys.Expense.List)), Times.Once);
        }
    }
}
