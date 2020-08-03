namespace Expensely.Application.Abstractions.Cryptography
{
    /// <summary>
    /// Represents the password verification result.
    /// </summary>
    public enum PasswordVerificationResult
    {
        /// <summary>
        /// Password verification was unsuccessful.
        /// </summary>
        Failure = 0,

        /// <summary>
        /// Password verification was successful.
        /// </summary>
        Success = 1
    }
}
