using Expensely.Application.Authentication.Commands.Register;
using Expensely.Domain;
using FluentValidation.TestHelper;
using Xunit;

namespace Expensely.Application.UnitTests.Authentication.Validators
{
    public class RegisterCommandValidatorTests
    {
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string Email = "Email";
        private const string Password = "Password";

        [Fact]
        public void Should_fail_if_first_name_is_null()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(null, LastName, Email, Password, Password);

            validator.ShouldHaveValidationErrorFor(x => x.FirstName, command).WithErrorCode(Errors.FirstName.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_first_name_is_empty()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(string.Empty, LastName, Email, Password, Password);

            validator.ShouldHaveValidationErrorFor(x => x.FirstName, command).WithErrorCode(Errors.FirstName.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_last_name_is_null()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(FirstName, null, Email, Password, Password);

            validator.ShouldHaveValidationErrorFor(x => x.LastName, command).WithErrorCode(Errors.LastName.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_last_name_is_empty()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(FirstName, string.Empty, Email, Password, Password);

            validator.ShouldHaveValidationErrorFor(x => x.LastName, command).WithErrorCode(Errors.LastName.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_email_is_null()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(FirstName, LastName, null, Password, Password);

            validator.ShouldHaveValidationErrorFor(x => x.Email, command).WithErrorCode(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_email_is_empty()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(FirstName, LastName, string.Empty, Password, Password);

            validator.ShouldHaveValidationErrorFor(x => x.Email, command).WithErrorCode(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_password_is_null()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(FirstName, LastName, Email, null, Password);

            validator.ShouldHaveValidationErrorFor(x => x.Password, command).WithErrorCode(Errors.Password.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_password_is_empty()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(FirstName, LastName, Email, string.Empty, Password);

            validator.ShouldHaveValidationErrorFor(x => x.Password, command).WithErrorCode(Errors.Password.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_confirm_password_is_null()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(FirstName, LastName, Email, Password, null);

            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, command).WithErrorCode(Errors.Password.NullOrEmpty);
        }

        [Fact]
        public void Should_fail_if_confirm_password_is_empty()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(FirstName, LastName, Email, Password, string.Empty);

            validator.ShouldHaveValidationErrorFor(x => x.ConfirmPassword, command).WithErrorCode(Errors.Password.NullOrEmpty);
        }

        [Fact]
        public void Should_succeed_if_command_is_valid()
        {
            var validator = new RegisterCommandValidator();
            var command = new RegisterCommand(FirstName, LastName, Email, Password, Password);

            TestValidationResult<RegisterCommand> result = validator.TestValidate(command);

            result.ShouldNotHaveValidationErrorFor(x => x.FirstName);
            result.ShouldNotHaveValidationErrorFor(x => x.LastName);
            result.ShouldNotHaveValidationErrorFor(x => x.Email);
            result.ShouldNotHaveValidationErrorFor(x => x.Password);
            result.ShouldNotHaveValidationErrorFor(x => x.ConfirmPassword);
        }
    }
}
