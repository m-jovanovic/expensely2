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
            RuleFor(x => x.Email).NotEmpty().WithErrorCode(Errors.Email.NullOrEmpty);

            RuleFor(x => x.Password).NotEmpty().WithErrorCode(Errors.Password.NullOrEmpty);

            RuleFor(x => x.ConfirmPassword).NotEmpty().WithErrorCode(Errors.Password.NullOrEmpty);

            RuleFor(x => x.FirstName).NotEmpty().WithErrorCode(Errors.FirstName.NullOrEmpty);

            RuleFor(x => x.LastName).NotEmpty().WithErrorCode(Errors.LastName.NullOrEmpty);
        }
    }
}
