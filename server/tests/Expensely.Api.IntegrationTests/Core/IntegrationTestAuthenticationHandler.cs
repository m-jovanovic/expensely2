using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Expensely.Infrastructure.Authentication.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Expensely.Api.IntegrationTests.Core
{
    public class IntegrationTestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public IntegrationTestAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        /// <inheritdoc />
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Claim[] claims =
            {
                new Claim(ExpenselyJwtClaimTypes.UserId, TestData.UserId.ToString()), 
                new Claim(ExpenselyJwtClaimTypes.Permissions, Options.ClaimsIssuer)
            };

            var identity = new ClaimsIdentity(claims, "Test");

            var principal = new ClaimsPrincipal(identity);

            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
