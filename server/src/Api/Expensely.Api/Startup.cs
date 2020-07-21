using Expensely.Application;
using Expensely.Infrastructure.Authentication;
using Expensely.Infrastructure.Authorization;
using Expensely.Infrastructure.Persistence;
using Expensely.Infrastructure.Services;
using Expensely.Migrations.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly:ApiController]

namespace Expensely.Api
{
    public class Startup
    {
        private const string ConnectionStringSettings = "ExpenselyDb";
        private const string CorsSettings = "Cors:AllowedOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(Configuration);

            services.AddPersistence(Configuration);

            services.AddAuthentication(Configuration);

            services.AddPermissionAuthorization();

            services.AddServices();

            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ExecuteMigrations(Configuration.GetConnectionString(ConnectionStringSettings));

            app.UseCors(configurePolicy =>
            {
                string origins = Configuration.GetValue<string>(CorsSettings);

                configurePolicy
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
