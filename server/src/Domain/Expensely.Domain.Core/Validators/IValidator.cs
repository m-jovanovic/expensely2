using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.Core.Validators
{
    public interface IValidator<T>
        where T : class
    {
        IValidator<T> SetNext(IValidator<T> next);

        Result Validate(T? request);
    }
}
