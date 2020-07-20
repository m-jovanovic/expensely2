using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Email
{
    public class EmailLengthValidator : Validator<string>
    {
        public const int MaxEmailLength = 256;

        public override Result Validate(string? request)
        {
            return request?.Length > MaxEmailLength ? Result.Fail(Errors.Email.LongerThanAllowed) : base.Validate(request);
        }
    }
}
