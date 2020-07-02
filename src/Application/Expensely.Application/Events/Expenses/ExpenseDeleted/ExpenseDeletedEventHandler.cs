using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Caching;
using Expensely.Application.Constants;
using Expensely.Application.Messaging;

namespace Expensely.Application.Events.Expenses.ExpenseDeleted
{
    public class ExpenseDeletedEventHandler : IEventHandler<ExpenseDeletedEvent>
    {
        private readonly ICacheService _cacheService;

        public ExpenseDeletedEventHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public Task Handle(ExpenseDeletedEvent notification, CancellationToken cancellationToken)
        {
            _cacheService.RemoveValue(CacheKeys.Expenses);

            _cacheService.RemoveValue(string.Format(CacheKeys.ExpenseById, notification.ExpenseId));

            return Task.CompletedTask;
        }
    }
}