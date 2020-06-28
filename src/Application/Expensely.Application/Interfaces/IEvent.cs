using MediatR;

namespace Expensely.Application.Interfaces
{
    /// <summary>
    /// Represents the marker interface for an event.
    /// </summary>
    public interface IEvent : INotification
    {
    }
}