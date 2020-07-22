using System;
using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordHasUppercaseLetterValidator : Validator<string>
    {
        private readonly Func<char, bool> _isUppercaseLetter = c => char.IsLetter(c) && char.IsUpper(c);

        public override Result Validate(string? request) =>
            !request?.Any(_isUppercaseLetter) ?? false ?
                Result.Fail(Errors.Password.MissingUppercaseLetter) : base.Validate(request);
    }
}