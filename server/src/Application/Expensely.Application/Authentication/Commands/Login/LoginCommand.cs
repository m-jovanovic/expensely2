﻿using Expensely.Application.Messaging;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Application.Authentication.Commands.Login
{
    /// <summary>
    /// Represents the command for logging in a user.
    /// </summary>
    public sealed class LoginCommand : ICommand<Result<string>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommand"/> class.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        /// <summary>
        /// Gets the email.
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public string Password { get; }
    }
}
