namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the login request.
    /// </summary>
    public sealed class LoginRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequest"/> class.
        /// </summary>
        public LoginRequest()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
    }
}
