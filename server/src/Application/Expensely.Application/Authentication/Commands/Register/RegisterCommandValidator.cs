using Expensely.Application.Utilities;
using Expensely.Domain;
using FluentValidation;

namespace Expensely.Application.Authentication.Commands.Register
{
    /// <summary>
    /// Represents the validator for the <see cref="RegisterCommand"/>.
    /// </summary>
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandValidator"/> class.
        /// </summary>
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithError(Errors.FirstName.NullOrEmpty);

            RuleFor(x => x.LastName).NotEmpty().WithError(Errors.LastName.NullOrEmpty);

            RuleFor(x => x.Email).NotEmpty().WithError(Errors.Email.NullOrEmpty);

            RuleFor(x => x.Password).NotEmpty().WithError(Errors.Password.NullOrEmpty);

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithError(Errors.Password.NullOrEmpty);

            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithError(Errors.Authentication.PasswordsDoNotMatch);
        }
    }
}
