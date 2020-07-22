using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Email
{
    public class EmailMaxLengthValidator : Validator<string>
    {
        public const int MaxEmailLength = 256;

        public override Result Validate(string? request) =>
            request?.Length > MaxEmailLength ? Result.Fail(Errors.Email.LongerThanAllowed) : base.Validate(request);
    }
}
