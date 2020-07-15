using System;
using System.Threading.Tasks;
using Expensely.Application.Caching;
using Expensely.Application.Constants;
using Expensely.Application.Expenses.Events.ExpenseCreated;
using Moq;
using Xunit;

namespace Expensely.Application.Tests.Expenses.Events
{
    public class ExpenseCreatedEventTests
    {
        [Fact]
        public async Task Handle_should_call_Remove_on_CacheService()
        {
            var cacheServiceMock = new Mock<ICacheService>();
            var handler = new ExpenseCreatedEventHandler(cacheServiceMock.Object);
            var expenseCreated = new ExpenseCreatedEvent(Guid.Empty);

            await handler.Handle(expenseCreated, default);

            cacheServiceMock.Verify(x => x.RemoveValue(It.Is<string>(s => s == CacheKeys.Expenses)));
        }
    }
}
