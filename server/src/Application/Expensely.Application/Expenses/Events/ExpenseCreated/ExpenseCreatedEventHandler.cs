using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Caching;
using Expensely.Application.Constants;
using Expensely.Application.Messaging;

namespace Expensely.Application.Expenses.Events.ExpenseCreated
{
    /// <summary>
    /// Represents the <see cref="ExpenseCreatedEvent"/> handler.
    /// </summary>
    internal sealed class ExpenseCreatedEventHandler : IEventHandler<ExpenseCreatedEvent>
    {
        private readonly ICacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        public ExpenseCreatedEventHandler(ICacheService cacheService) => _cacheService = cacheService;

        /// <inheritdoc />
        public Task Handle(ExpenseCreatedEvent notification, CancellationToken cancellationToken)
        {
            _cacheService.RemoveValue(CacheKeys.Expenses);

            return Task.CompletedTask;
        }
    }
}
