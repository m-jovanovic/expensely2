using System.Text.RegularExpressions;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Email
{
    /// <summary>
    /// Validates that the email is of a valid format.
    /// </summary>
    public sealed class EmailFormatValidator : Validator<string>
    {
        private const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        /// <inheritdoc />
        public override Result Validate(string? item) =>
            !Regex.IsMatch(item, EmailRegexPattern, RegexOptions.IgnoreCase) ?
                Result.Fail(Errors.Email.InvalidFormat) : base.Validate(item);
    }
}
