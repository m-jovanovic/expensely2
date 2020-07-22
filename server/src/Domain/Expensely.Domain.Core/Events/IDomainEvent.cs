using MediatR;

namespace Expensely.Domain.Core.Events
{
    /// <summary>
    /// Represents the marker interface for a domain event.
    /// </summary>
    public interface IDomainEvent : INotification
    {
    }
}
