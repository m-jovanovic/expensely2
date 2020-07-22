using System;
using System.Threading.Tasks;
using MediatR;

namespace Expensely.Domain.Core.Events
{
    /// <summary>
    /// Represents the domain events dispatcher.
    /// </summary>
    public static class DomainEvents
    {
        /// <summary>
        /// Gets or sets the mediator factory function.
        /// </summary>
        public static Func<IMediator>? Mediator { get; set; }

        /// <summary>
        /// Raises the specified domain event.
        /// </summary>
        /// <typeparam name="T">The domain event type.</typeparam>
        /// <param name="domainEvent">The domain event to be raised.</param>
        /// <returns>Returns a task.</returns>
        public static async Task Raise<T>(T domainEvent)
            where T : IDomainEvent
        {
            IMediator mediator = Mediator!();

            await mediator.Publish(domainEvent);
        }
    }
}
