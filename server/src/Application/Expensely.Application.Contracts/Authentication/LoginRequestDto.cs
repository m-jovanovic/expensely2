namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the login request DTO.
    /// </summary>
    public sealed class LoginRequestDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequestDto"/> class.
        /// </summary>
        public LoginRequestDto()
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
