using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Caching;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Constants;

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
            _cacheService.RemoveValue(CacheKeys.Expense.List);

            return Task.CompletedTask;
        }
    }
}
