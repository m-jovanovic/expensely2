using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.Core.Handlers
{
    public abstract class Handler<T> : IHandler<T>
        where T : class
    {
        private IHandler<T>? _next;

        public Result Handle(T request)
        {
           return _next?.Handle(request) ?? Result.Fail("No handler was found.");
        }

        public IHandler<T> SetNext(IHandler<T> next)
        {
            _next = next;

            return _next;
        }
    }
}
