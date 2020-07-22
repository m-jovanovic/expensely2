using System.Text.RegularExpressions;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Email
{
    public class EmailFormatValidator : Validator<string>
    {
        private const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public override Result Validate(string? item) =>
            !Regex.IsMatch(item, EmailRegexPattern, RegexOptions.IgnoreCase) ?
                Result.Fail(Errors.Email.IncorrectFormat) : base.Validate(item);
    }
}
