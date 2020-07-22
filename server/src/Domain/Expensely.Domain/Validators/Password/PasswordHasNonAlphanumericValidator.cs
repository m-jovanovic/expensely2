using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordHasNonAlphanumericValidator : Validator<string>
    {
        public override Result Validate(string? item) =>
            item?.All(IsLetterOrDigit) ?? false ? Result.Fail(Errors.Password.MissingNonAlphaNumeric) : base.Validate(item);

        private static bool IsLetterOrDigit(char c) => IsLower(c) || IsUpper(c) || IsDigit(c);

        private static bool IsLower(char c) => c >= 'a' && c <= 'z';

        private static bool IsUpper(char c) => c >= 'A' && c <= 'Z';

        private static bool IsDigit(char c) => c >= '0' && c <= '9';
    }
}