using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Expensely.Authentication.Abstractions;
using Expensely.Authentication.Cryptography;
using Expensely.Authentication.Options;
using Expensely.Common.Contracts.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Authentication.Services
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IClaimsProvider _claimsProvider;

        public AuthenticationService(
            IOptions<JwtOptions> jwtOptions,
            IUserService userService,
            IPasswordHasher passwordHasher,
            IClaimsProvider claimsProvider)
        {
            _jwtOptions = jwtOptions.Value;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _claimsProvider = claimsProvider;
        }

        /// <inheritdoc />
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            string[] errorCodes = await _userService.CreateAsync(registerRequest.Email, registerRequest.Password);

            if (errorCodes.Length == 0)
            {
                return new RegisterResponse(true, null);
            }

            return new RegisterResponse(false, errorCodes);
        }

        /// <inheritdoc />
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            dynamic user = await _userService.GetByEmailAsync(loginRequest.Email);

            if (user is null)
            {
                // TODO: Introduce class for error codes.
                return LoginResponse.Failed("UserNotFound");
            }

            if (_passwordHasher.VerifyHashedPassword(user.PasswordHash, loginRequest.Password) ==
                PasswordVerificationResult.Failure)
            {
                // TODO: Introduce class for error codes.
                return LoginResponse.Failed("InvalidPassword");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = await _claimsProvider.GetClaimsAsync(user);

            DateTime tokenExpirationTime = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationInMinutes);

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                null,
                tokenExpirationTime,
                signingCredentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return LoginResponse.Successful(tokenString);
        }
    }
}