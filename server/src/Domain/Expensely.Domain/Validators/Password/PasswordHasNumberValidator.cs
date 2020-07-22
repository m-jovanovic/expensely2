using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordHasNumberValidator : Validator<string>
    {
        public override Result Validate(string? request) =>
            !request?.Any(char.IsNumber) ?? false ? Result.Fail(Errors.Password.MissingNumber) : base.Validate(request);
    }
}