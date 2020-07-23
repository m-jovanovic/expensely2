using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Caching;
using Expensely.Application.Constants;
using Expensely.Application.Messaging;

namespace Expensely.Application.Expenses.Events.ExpenseDeleted
{
    /// <summary>
    /// Represents the <see cref="ExpenseDeletedEvent"/> handler.
    /// </summary>
    internal class ExpenseDeletedEventHandler : IEventHandler<ExpenseDeletedEvent>
    {
        private readonly ICacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseDeletedEventHandler"/> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        public ExpenseDeletedEventHandler(ICacheService cacheService) => _cacheService = cacheService;

        /// <inheritdoc />
        public Task Handle(ExpenseDeletedEvent notification, CancellationToken cancellationToken)
        {
            _cacheService.RemoveValue(CacheKeys.Expenses);

            _cacheService.RemoveValue(string.Format(CacheKeys.ExpenseById, notification.ExpenseId));

            return Task.CompletedTask;
        }
    }
}