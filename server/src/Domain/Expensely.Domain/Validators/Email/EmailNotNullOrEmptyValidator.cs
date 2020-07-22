using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Email
{
    public class EmailNotNullOrEmptyValidator : Validator<string>
    {
        public override Result Validate(string? request) =>
            string.IsNullOrWhiteSpace(request) ? Result.Fail(Errors.Email.NullOrEmpty) : base.Validate(request);
    }
}
