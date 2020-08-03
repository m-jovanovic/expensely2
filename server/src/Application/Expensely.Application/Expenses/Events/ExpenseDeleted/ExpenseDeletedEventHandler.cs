using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Caching;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Constants;

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
            _cacheService.RemoveValue(CacheKeys.Expense.List);

            _cacheService.RemoveValue(string.Format(CacheKeys.Expense.ById, notification.ExpenseId));

            return Task.CompletedTask;
        }
    }
}