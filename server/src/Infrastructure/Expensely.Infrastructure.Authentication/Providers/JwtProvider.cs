using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Authentication;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Domain.Users;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authentication.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Infrastructure.Authentication.Providers
{
    /// <summary>
    /// Represents the JWT provider.
    /// </summary>
    internal sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IDateTime _dateTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtProvider"/> class.
        /// </summary>
        /// <param name="jwtOptions">The JWT options.</param>
        /// <param name="claimsProvider">The claims provider.</param>
        /// <param name="dateTime">The current date and time.</param>
        public JwtProvider(
            IOptions<JwtOptions> jwtOptions,
            IClaimsProvider claimsProvider,
            IDateTime dateTime)
        {
            _claimsProvider = claimsProvider;
            _dateTime = dateTime;
            _jwtOptions = jwtOptions.Value;
        }

        /// <inheritdoc />
        public async Task<TokenResponse> CreateAsync(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = await _claimsProvider.GetClaimsAsync(user);

            DateTime tokenExpirationTime = _dateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationInMinutes);

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                null,
                tokenExpirationTime,
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return new TokenResponse(tokenValue);
        }
    }
}
