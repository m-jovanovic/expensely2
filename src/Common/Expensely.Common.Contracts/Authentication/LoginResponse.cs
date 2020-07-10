namespace Expensely.Common.Contracts.Authentication
{
    public class LoginResponse
    {
        private LoginResponse(bool success, string token, string? errorCode)
        {
            Success = success;
            ErrorCode = errorCode;
            Token = token;
        }

        public bool Success { get; }

        public string? ErrorCode { get; set; }

        public string Token { get; }

        public static LoginResponse Successful(string token) => new LoginResponse(true, token, null);

        public static LoginResponse Failed(string errorCode) => new LoginResponse(false, string.Empty, errorCode);
    }
}
