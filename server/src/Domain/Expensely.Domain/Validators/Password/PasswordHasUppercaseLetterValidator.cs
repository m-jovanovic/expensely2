using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordHasUppercaseLetterValidator : Validator<string>
    {
        public override Result Validate(string? request) =>
            !request?.Any(IsUpper) ?? false ?
                Result.Fail(Errors.Password.MissingUppercaseLetter) : base.Validate(request);

        private static bool IsUpper(char c) => c >= 'A' && c <= 'Z';
    }
}