﻿using Expensely.Application.Authentication.Commands.Login;
using Expensely.Domain;
using FluentValidation.TestHelper;
using Xunit;

namespace Expensely.Application.UnitTests.Authentication.Validators
{
    public class LoginCommandValidatorTests
    {
        private const string Email = "Email";
        private const string Password = "Password";

        [Fact]
        public void Should_fail_if_email_is_null()
        {
            var validator = new LoginCommandValidator();
            var command = new LoginCommand(null, Password);

            validator.ShouldHaveValidationErrorFor(x => x.Email, command).WithErrorCode(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_email_is_empty()
        {
            var validator = new LoginCommandValidator();
            var command = new LoginCommand(string.Empty, Password);

            validator.ShouldHaveValidationErrorFor(x => x.Email, command).WithErrorCode(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_password_is_null()
        {
            var validator = new LoginCommandValidator();
            var command = new LoginCommand(Email, null);

            validator.ShouldHaveValidationErrorFor(x => x.Password, command).WithErrorCode(Errors.Password.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_password_is_empty()
        {
            var validator = new LoginCommandValidator();
            var command = new LoginCommand(Email, string.Empty);

            validator.ShouldHaveValidationErrorFor(x => x.Password, command).WithErrorCode(Errors.Password.NullOrEmpty);
        }

        [Fact]
        public void Should_succeed_if_command_is_valid()
        {
            var validator = new LoginCommandValidator();
            var command = new LoginCommand(Email, Password);

            TestValidationResult<LoginCommand> result = validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
        }
    }
}
