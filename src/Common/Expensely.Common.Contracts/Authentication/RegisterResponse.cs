namespace Expensely.Common.Contracts.Authentication
{
    public class RegisterResponse
    {
        public RegisterResponse(bool success, string[]? errorCodes)
        {
            Success = success;
            ErrorCodes = errorCodes;
        }

        public bool Success { get; }

        public string[]? ErrorCodes { get; }
    }
}
