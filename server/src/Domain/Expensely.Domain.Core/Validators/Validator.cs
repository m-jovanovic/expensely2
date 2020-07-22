using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.Core.Validators
{
    public abstract class Validator<T> : IValidator<T>
        where T : class
    {
        private IValidator<T>? _next;

        public IValidator<T> SetNext(IValidator<T> next) => _next = next;

        public virtual Result Validate(T? request) => _next?.Validate(request) ?? Result.Ok();
    }
}
