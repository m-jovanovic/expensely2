namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the register request.
    /// </summary>
    public sealed class RegisterRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterRequest"/> class.
        /// </summary>
        public RegisterRequest()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirmed password.
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}
