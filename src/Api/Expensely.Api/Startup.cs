using Expensely.Application;
using Expensely.Authentication;
using Expensely.Infrastructure;
using Expensely.Migrations.Core.Extensions;
using Expensely.Persistence;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(Configuration);

            services.AddCustomAuthentication(Configuration);

            services.AddInfrastructure();

            services.AddPersistence(Configuration);

            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ExecuteMigrations(Configuration.GetConnectionString("ExpenselyDb"));

            app.UseCors(configurePolicy =>
            {
                string origins = Configuration.GetValue<string>("Cors:AllowedOrigins");

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
