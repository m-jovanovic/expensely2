using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.Core.Handlers
{
    public interface IHandler<T>
        where T : class
    {
        Result Handle(T request);
    }
}
