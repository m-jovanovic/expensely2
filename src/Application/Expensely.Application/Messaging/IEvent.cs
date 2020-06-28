using MediatR;

namespace Expensely.Application.Messaging
{
    /// <summary>
    /// Represents the marker interface for an event.
    /// </summary>
    public interface IEvent : INotification
    {
    }
}