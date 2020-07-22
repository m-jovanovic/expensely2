using System;
using System.Linq;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordHasLowercaseLetterValidator : Validator<string>
    {
        private readonly Func<char, bool> _isLowercaseLetter = c => char.IsLetter(c) && char.IsLower(c);

        public override Result Validate(string? request) =>
            !request?.Any(_isLowercaseLetter) ?? false ?
                Result.Fail(Errors.Password.MissingLowercaseLetter) : base.Validate(request);
    }
}