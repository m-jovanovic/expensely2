namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the register request DTO..
    /// </summary>
    public sealed class RegisterRequestDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterRequestDto"/> class.
        /// </summary>
        public RegisterRequestDto()
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
