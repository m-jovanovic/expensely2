using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authentication.Cryptography;
using Expensely.Infrastructure.Authentication.Entities;
using Expensely.Infrastructure.Authentication.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Infrastructure.Authentication.Services
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
        public async Task<Result<string>> RegisterAsync(string firstName, string lastName, Email email, string password)
        {
            Result<AuthenticatedUser> result = await _userService.CreateAsync(firstName, lastName, email, password);

            if (result.IsFailure)
            {
                return Result.Fail<string>(result.Error);
            }

            AuthenticatedUser authenticatedUser = result.Value();

            string jwt = await CreateJwtAsync(authenticatedUser);

            return Result.Ok(jwt);
        }

        /// <inheritdoc />
        public async Task<Result<string>> LoginAsync(string email, string password)
        {
            AuthenticatedUser? authenticatedUser = await _userService.GetByEmailAsync(email);

            if (authenticatedUser is null)
            {
                return Result.Fail<string>(Errors.Authentication.UserNotFound);
            }

            PasswordVerificationResult passwordVerificationResult = _passwordHasher
                .VerifyHashedPassword(authenticatedUser.PasswordHash, password);

            if (passwordVerificationResult == PasswordVerificationResult.Failure)
            {
                return Result.Fail<string>(Errors.Authentication.InvalidPassword);
            }

            string jwt = await CreateJwtAsync(authenticatedUser);

            return Result.Ok(jwt);
        }

        private async Task<string> CreateJwtAsync(AuthenticatedUser authenticatedUser)
        {
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

            return tokenString;
        }
    }
}