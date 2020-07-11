using System;
using System.Threading.Tasks;
using MediatR;

namespace Expensely.Domain.Core.Events
{
    public static class DomainEvents
    {
        public static Func<IMediator>? Mediator { get; set; }

        public static async Task Raise<T>(T domainEvent)
            where T : IDomainEvent
        {
            IMediator mediator = Mediator!();

            await mediator.Publish(domainEvent);
        }
    }
}
