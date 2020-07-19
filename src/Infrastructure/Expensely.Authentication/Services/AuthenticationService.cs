using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Expensely.Authentication.Abstractions;
using Expensely.Authentication.Cryptography;
using Expensely.Authentication.Entities;
using Expensely.Authentication.Options;
using Expensely.Common.Contracts.Authentication;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
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
        public async Task<Result> RegisterAsync(RegisterRequest registerRequest)
        {
            Result result = await _userService.CreateAsync(
                registerRequest.FirstName,
                registerRequest.LastName,
                registerRequest.Email,
                registerRequest.Password);

            return result;
        }

        /// <inheritdoc />
        public async Task<Result<string>> LoginAsync(LoginRequest loginRequest)
        {
            AuthenticatedUser authenticatedUser = await _userService.GetByEmailAsync(loginRequest.Email);

            if (authenticatedUser is null)
            {
                return Result.Fail<string>(Errors.Authentication.UserNotFound);
            }

            if (_passwordHasher.VerifyHashedPassword(authenticatedUser.PasswordHash, loginRequest.Password) ==
                PasswordVerificationResult.Failure)
            {
                return Result.Fail<string>(Errors.Authentication.InvalidPassword);
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            Claim[] claims = await _claimsProvider.GetClaimsAsync(authenticatedUser);

            DateTime tokenExpirationTime = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationInMinutes);

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                null,
                tokenExpirationTime,
                signingCredentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Result.Ok(tokenString);
        }
    }
}