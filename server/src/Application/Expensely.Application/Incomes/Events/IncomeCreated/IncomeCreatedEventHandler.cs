using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Caching;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Constants;

namespace Expensely.Application.Incomes.Events.IncomeCreated
{
    /// <summary>
    /// Represents the <see cref="IncomeCreatedEvent"/> handler.
    /// </summary>
    internal sealed class IncomeCreatedEventHandler : IEventHandler<IncomeCreatedEvent>
    {
        private readonly ICacheService _cacheService;
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        public IncomeCreatedEventHandler(ICacheService cacheService, IUserIdentifierProvider userIdentifierProvider)
        {
            _cacheService = cacheService;
            _userIdentifierProvider = userIdentifierProvider;
        }

        /// <inheritdoc />
        public Task Handle(IncomeCreatedEvent notification, CancellationToken cancellationToken)
        {
            _cacheService.RemoveByPattern(string.Format(CacheKeys.Transactions.CacheKeyPrefix, _userIdentifierProvider.UserId));

            _cacheService.RemoveByPattern(string.Format(CacheKeys.Incomes.CacheKeyPrefix, _userIdentifierProvider.UserId));

            return Task.CompletedTask;
        }
    }
}
