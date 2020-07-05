namespace Expensely.Contracts.Authentication
{
    public class RegisterUserRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
