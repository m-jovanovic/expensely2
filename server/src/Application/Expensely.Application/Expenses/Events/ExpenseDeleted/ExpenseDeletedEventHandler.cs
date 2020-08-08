using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
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
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseDeletedEventHandler"/> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        public ExpenseDeletedEventHandler(ICacheService cacheService, IUserIdentifierProvider userIdentifierProvider)
        {
            _cacheService = cacheService;
            _userIdentifierProvider = userIdentifierProvider;
        }

        /// <inheritdoc />
        public Task Handle(ExpenseDeletedEvent notification, CancellationToken cancellationToken)
        {
            _cacheService.RemoveByPattern(string.Format(CacheKeys.Expense.CacheKeyPrefix, _userIdentifierProvider.UserId));

            return Task.CompletedTask;
        }
    }
}