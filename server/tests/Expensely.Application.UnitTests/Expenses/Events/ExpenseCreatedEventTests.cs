using System;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Caching;
using Expensely.Application.Constants;
using Expensely.Application.Expenses.Events.ExpenseCreated;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Expenses.Events
{
    public class ExpenseCreatedEventTests
    {
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;

        public ExpenseCreatedEventTests()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
        }

        [Fact]
        public async Task Handle_should_call_remove_by_pattern_on_cache_service()
        {
            var userId = Guid.NewGuid();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(userId);
            string cacheKeyPattern = string.Format(CacheKeys.Expense.CacheKeyPrefix, userId);
            var eventHandler = new ExpenseCreatedEventHandler(_cacheServiceMock.Object, _userIdentifierProviderMock.Object);
            var @event = new ExpenseCreatedEvent(Guid.Empty);

            await eventHandler.Handle(@event, default);

            _cacheServiceMock.Verify(x => x.RemoveByPattern(It.Is<string>(s => s == cacheKeyPattern)), Times.Once);
        }
    }
}
