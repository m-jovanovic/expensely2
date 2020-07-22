using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordHasLowercaseLetterValidator : Validator<string>
    {
        public override Result Validate(string? item) =>
            !item?.Any(IsLower) ?? false ?
                Result.Fail(Errors.Password.MissingLowercaseLetter) : base.Validate(item);

        private static bool IsLower(char c) => c >= 'a' && c <= 'z';
    }
}