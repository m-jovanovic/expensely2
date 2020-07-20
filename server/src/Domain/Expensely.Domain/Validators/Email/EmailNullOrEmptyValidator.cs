using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Email
{
    public class EmailNullOrEmptyValidator : Validator<string>
    {
        public override Result Validate(string? request)
        {
            return string.IsNullOrWhiteSpace(request) ? Result.Fail(Errors.Email.NullOrEmpty) : base.Validate(request);
        }
    }
}
