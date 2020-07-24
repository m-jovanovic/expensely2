namespace Expensely.Application.Contracts.Authentication
{
    /// <summary>
    /// Represents the JWT token response.
    /// </summary>
    public sealed class TokenResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenResponse"/> class.
        /// </summary>
        /// <param name="token">The JWT.</param>
        public TokenResponse(string token) => Token = token;

        /// <summary>
        /// Gets the token.
        /// </summary>
        public string Token { get; }
    }
}
