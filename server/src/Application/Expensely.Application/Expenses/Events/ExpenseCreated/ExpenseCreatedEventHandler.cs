﻿using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Caching;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Constants;

namespace Expensely.Application.Expenses.Events.ExpenseCreated
{
    /// <summary>
    /// Represents the <see cref="ExpenseCreatedEvent"/> handler.
    /// </summary>
    internal sealed class ExpenseCreatedEventHandler : IEventHandler<ExpenseCreatedEvent>
    {
        private readonly ICacheService _cacheService;
        private readonly IUserIdentifierProvider _userIdentifierProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpenseCreatedEventHandler"/> class.
        /// </summary>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        public ExpenseCreatedEventHandler(ICacheService cacheService, IUserIdentifierProvider userIdentifierProvider)
        {
            _cacheService = cacheService;
            _userIdentifierProvider = userIdentifierProvider;
        }

        /// <inheritdoc />
        public Task Handle(ExpenseCreatedEvent notification, CancellationToken cancellationToken)
        {
            _cacheService.RemoveByPattern(string.Format(CacheKeys.Transactions.CacheKeyPrefix, _userIdentifierProvider.UserId));

            _cacheService.RemoveByPattern(string.Format(CacheKeys.Expenses.CacheKeyPrefix, _userIdentifierProvider.UserId));

            return Task.CompletedTask;
        }
    }
}
