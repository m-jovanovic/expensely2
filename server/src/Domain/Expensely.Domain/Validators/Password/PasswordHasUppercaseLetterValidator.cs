using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordHasUppercaseLetterValidator : Validator<string>
    {
        public override Result Validate(string? item) =>
            !item?.Any(IsUpper) ?? false ?
                Result.Fail(Errors.Password.MissingUppercaseLetter) : base.Validate(item);

        private static bool IsUpper(char c) => c >= 'A' && c <= 'Z';
    }
}