﻿using Expensely.Domain;
using FluentValidation;

namespace Expensely.Application.Authentication.Commands.Login
{
    /// <summary>
    /// Represents the <see cref="LoginCommand"/> validator.
    /// </summary>
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandValidator"/> class.
        /// </summary>
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotNull().WithErrorCode(Errors.Email.NullOrEmpty);
            RuleFor(x => x.Email).NotEmpty().WithErrorCode(Errors.Email.NullOrEmpty);
            RuleFor(x => x.Password).NotNull().WithErrorCode(Errors.Password.NullOrEmpty);
            RuleFor(x => x.Password).NotEmpty().WithErrorCode(Errors.Password.NullOrEmpty);
        }
    }
}
