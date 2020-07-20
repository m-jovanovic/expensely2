using System;

namespace Expensely.Common.Contracts.Authentication
{
    public class RegisterRequest
    {
        public RegisterRequest()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}
