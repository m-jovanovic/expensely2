using Expensely.Application;
using Expensely.Infrastructure;
using Expensely.Migrations.Extensions;
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
            services.AddControllers();

            services.AddMemoryCache();

            services.AddApplication();

            services.AddInfrastructure();

            services.AddPersistence(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction())
            {
                app.ExecuteMigrations(Configuration.GetConnectionString("ExpenselyDb"));
            }

            app.UseCors(configurePolicy =>
            {
                string origins = Configuration.GetValue<string>("Cors:AllowedOrigins");

                configurePolicy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins(origins);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
