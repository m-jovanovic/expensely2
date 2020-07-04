namespace Expensely.Presentation.Services.Authentication
{
    public class AccessToken
    {
        public const string Key = "AccessToken";

        public AccessToken()
        {
            Token = string.Empty;
        }

        public string Token { get; set; }
    }
}
