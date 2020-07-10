using System;
using System.Text;
using Expensely.Authentication.Factories;
using Expensely.Authentication.Interfaces;
using Expensely.Authentication.Options;
using Expensely.Authentication.Permissions;
using Expensely.Authentication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
                o.UseSqlServer(configuration.GetConnectionString(ExpenselyDb)));

            services.AddDefaultIdentity<IdentityUser>(o =>
                {
                    o.User.RequireUniqueEmail = true;

                    // TODO: Consider enabling this for production?
                    // o.Stores.ProtectPersonalData = true;

                    // TODO: Implement support for this some time in the future.
                    // o.SignIn.RequireConfirmedAccount = true;
                    // o.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ExpenselyAuthenticationDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]))
                    };

                    // TODO: See if this is really necessary.
                    // options.SaveToken = true;
                });

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SettingsKey));
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.AddScoped<IUserClaimsPrincipalFactory<IdentityUser>, ExpenselyClaimsPrincipalFactory>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }
    }
}
