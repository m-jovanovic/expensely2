using System.Text;
using Expensely.Application.Abstractions;
using Expensely.Infrastructure.Authentication.Abstractions;
using Expensely.Infrastructure.Authentication.Constants;
using Expensely.Infrastructure.Authentication.Cryptography;
using Expensely.Infrastructure.Authentication.Options;
using Expensely.Infrastructure.Authentication.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Expensely.Infrastructure.Authentication
{
    public static class DependencyInjection
    {
        private const string ExpenselyDb = "ExpenselyDb";

        /// <summary>
        /// Registers the necessary services with the DI framework.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
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

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IRoleUniquenessChecker, RoleUniquenessChecker>();

            services.AddScoped<IClaimsProvider, ClaimsProvider>();
        }
    }
}
