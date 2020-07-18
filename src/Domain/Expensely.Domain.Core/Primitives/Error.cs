using System.Collections.Generic;

namespace Expensely.Domain.Core.Primitives
{
    public sealed class Error : ValueObject
    {
        public Error(string code)
        {
            Code = code;
        }

        public string Code { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
        }

        internal static Error None => new Error(string.Empty);
    }
}
