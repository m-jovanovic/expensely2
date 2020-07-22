using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordMinLengthValidator : Validator<string>
    {
        public const int MinPasswordLength = 6;

        public override Result Validate(string? request) =>
            request?.Length < MinPasswordLength ? Result.Fail(Errors.Password.TooShort) : base.Validate(request);
    }
}
