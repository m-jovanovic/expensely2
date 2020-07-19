namespace Expensely.Common.Contracts.Authentication
{
    public class RegisterResponse
    {
        public RegisterResponse(bool success, string? errorCode)
        {
            Success = success;
            ErrorCode = errorCode;
        }

        public bool Success { get; }

        public string? ErrorCode { get; }
    }
}
