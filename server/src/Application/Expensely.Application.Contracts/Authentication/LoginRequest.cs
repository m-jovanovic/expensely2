namespace Expensely.Application.Contracts.Authentication
{
    public class LoginRequest
    {
        public LoginRequest()
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
