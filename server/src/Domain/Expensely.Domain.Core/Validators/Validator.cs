using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.Core.Validators
{
    /// <summary>
    /// Represents the base class all validators derive from.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    public abstract class Validator<T> : IValidator<T>
        where T : class
    {
        private IValidator<T>? _next;

        /// <inheritdoc />
        public IValidator<T> SetNext(IValidator<T> next)
        {
            if (_next is null)
            {
                _next = next;
            }
            else
            {
                _next.SetNext(next);
            }

            return this;
        }

        /// <inheritdoc />
        public virtual Result Validate(T? item) => _next?.Validate(item) ?? Result.Ok();
    }
}
