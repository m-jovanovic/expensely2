using Expensely.Application.Abstractions.Messaging;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Application.Authentication.Commands.Register
{
    /// <summary>
    /// Represents the command for registering a user.
    /// </summary>
    public sealed class RegisterCommand : ICommand<Result>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommand"/> class.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <param name="confirmPassword">The confirmed password.</param>
        public RegisterCommand(string firstName, string lastName, string email, string password, string confirmPassword)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// Gets the confirmed password.
        /// </summary>
        public string ConfirmPassword { get; }
    }
}
