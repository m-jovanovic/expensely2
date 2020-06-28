using MediatR;

namespace Expensely.Application.Messaging
{
    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
        where TEvent : INotification
    {
    }
}