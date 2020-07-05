namespace Expensely.Contracts.Authentication
{
    public class RegisterUserResponse
    {
        public RegisterUserResponse(bool success, string[]? errorCodes)
        {
            Success = success;
            ErrorCodes = errorCodes;
        }

        public bool Success { get; }

        public string[]? ErrorCodes { get; }
    }
}
