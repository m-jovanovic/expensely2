using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordHasSymbolValidator : Validator<string>
    {
        public override Result Validate(string? request) =>
            !request?.Any(char.IsSymbol) ?? false ? Result.Fail(Errors.Password.MissingSymbol) : base.Validate(request);
    }
}