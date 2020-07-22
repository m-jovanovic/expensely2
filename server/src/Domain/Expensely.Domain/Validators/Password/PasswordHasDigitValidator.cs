using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordHasDigitValidator : Validator<string>
    {
        public override Result Validate(string? item) =>
            !item?.Any(IsDigit) ?? false ? Result.Fail(Errors.Password.MissingDigit) : base.Validate(item);

        private static bool IsDigit(char c) => c >= '0' && c <= '9';
    }
}