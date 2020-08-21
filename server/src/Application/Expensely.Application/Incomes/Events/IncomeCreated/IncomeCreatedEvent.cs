using System;
using Expensely.Application.Core.Abstractions.Messaging;

namespace Expensely.Application.Incomes.Events.IncomeCreated
{
    /// <summary>
    /// Represents the event that is raised when an income is created.
    /// </summary>
    public sealed class IncomeCreatedEvent : IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncomeCreatedEvent"/> class.
        /// </summary>
        /// <param name="incomeId">The income identifier.</param>
        public IncomeCreatedEvent(Guid incomeId) => IncomeId = incomeId;

        /// <summary>
        /// Gets the income identifier.
        /// </summary>
        public Guid IncomeId { get; }
    }
}
