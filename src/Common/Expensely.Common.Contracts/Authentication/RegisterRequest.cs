namespace Expensely.Common.Contracts.Authentication
{
    public class RegisterRequest
    {
        public RegisterRequest()
        {
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
