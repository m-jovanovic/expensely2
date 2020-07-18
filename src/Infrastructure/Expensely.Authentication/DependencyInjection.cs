using System.Text;
using Expensely.Authentication.Abstractions;
using Expensely.Authentication.Constants;
using Expensely.Authentication.Cryptography;
using Expensely.Authentication.Options;
using Expensely.Authentication.Services;
using Expensely.Common.Authorization.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Authentication
{
    public static class DependencyInjection
    {
        private const string ExpenselyDb = "ExpenselyDb";

        public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ExpenselyAuthenticationDbContext>(o =>
                o.UseNpgsql(configuration.GetConnectionString(ExpenselyDb)));

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
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
        }
    }
}
