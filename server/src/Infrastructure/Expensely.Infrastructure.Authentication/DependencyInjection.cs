using System.Text;
using Expensely.Application.Abstractions.Cryptography;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authentication.Constants;
using Expensely.Infrastructure.Authentication.Cryptography;
using Expensely.Infrastructure.Authentication.Options;
using Expensely.Infrastructure.Authentication.Permissions;
using Expensely.Infrastructure.Authentication.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Infrastructure.Authentication
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(ExpenselyJwtDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration[ExpenselyJwtDefaults.IssuerSettingsKey],
                        ValidAudience = configuration[ExpenselyJwtDefaults.AudienceSettingsKey],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration[ExpenselyJwtDefaults.SecurityKeySettingsKey]))
                    };
                });

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SettingsKey));

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IClaimsProvider, ClaimsProvider>();
        }
    }
}
