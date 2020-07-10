using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Expensely.Authentication.Interfaces;
using Expensely.Authentication.Options;
using Expensely.Common.Contracts.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Authentication.Services
{
    internal sealed class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IUserClaimsPrincipalFactory<IdentityUser> _claimsPrincipalFactory;

        public AuthenticationService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUserClaimsPrincipalFactory<IdentityUser> claimsPrincipalFactory,
            IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _claimsPrincipalFactory = claimsPrincipalFactory;
            _jwtOptions = jwtOptions.Value;
        }

        /// <inheritdoc />
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = new IdentityUser
            {
                UserName = registerRequest.Email,
                Email = registerRequest.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (result.Succeeded)
            {
                return new RegisterResponse(true, null);
            }

            string[] errorCodes = result.Errors.Select(x => x.Code).ToArray();

            return new RegisterResponse(false, errorCodes);
        }

        /// <inheritdoc />
        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false, true);

            if (!result.Succeeded)
            {
                return LoginResponse.Failed(result.ToString());
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            IdentityUser user = await _userManager.FindByEmailAsync(loginRequest.Email);

            ClaimsPrincipal claimsPrincipal = await _claimsPrincipalFactory.CreateAsync(user);

            DateTime tokenExpirationTime = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationInMinutes);

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claimsPrincipal.Claims,
                null,
                tokenExpirationTime,
                signingCredentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return LoginResponse.Successful(tokenString);
        }
    }
}