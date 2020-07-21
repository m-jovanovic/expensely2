using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Caching;
using Expensely.Application.Constants;
using Expensely.Application.Messaging;

namespace Expensely.Application.Expenses.Events.ExpenseCreated
{
    internal sealed class ExpenseCreatedEventHandler : IEventHandler<ExpenseCreatedEvent>
    {
        private readonly ICacheService _cacheService;

        public ExpenseCreatedEventHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public Task Handle(ExpenseCreatedEvent notification, CancellationToken cancellationToken)
        {
            _cacheService.RemoveValue(CacheKeys.Expenses);

            return Task.CompletedTask;
        }
    }
}
