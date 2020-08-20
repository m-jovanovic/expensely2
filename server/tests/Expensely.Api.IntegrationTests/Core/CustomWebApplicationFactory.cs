using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Expensely.Domain.Users;
using Expensely.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Expensely.Api.IntegrationTests.Core
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        /// <inheritdoc />
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseDefaultServiceProvider(s => s.ValidateScopes = false);

            builder.ConfigureAppConfiguration(configuration =>
            {
                configuration
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true);
            });

            builder.ConfigureServices(services =>
            {
                ServiceDescriptor descriptor = services
                    .SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ExpenselyDbContext>));

                services.Remove(descriptor);
                
                services.AddDbContext<ExpenselyDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Expensely");
                });

                ServiceProvider serviceProvider = services.BuildServiceProvider();

                using IServiceScope serviceScope = serviceProvider.CreateScope();

                IServiceProvider scopedServiceProvider = serviceScope.ServiceProvider;

                using ExpenselyDbContext dbContext = scopedServiceProvider.GetRequiredService<ExpenselyDbContext>();

                ILogger<CustomWebApplicationFactory> logger = scopedServiceProvider
                    .GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                dbContext.Database.EnsureCreated();

                try
                {
                    DatabaseInitializer
                        .InitializeDatabaseForTestsAsync(dbContext, scopedServiceProvider)
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception ex)
                {
                    logger.LogError(
                        ex,
                        "An error occurred seeding the database with test messages. Error: {Message}", ex.Message);
                }
            });
        }

        public HttpClient CreateClient(Permission permission)
        {
            return WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, IntegrationTestAuthenticationHandler>("Test", options =>
                        {
                            options.ClaimsIssuer = permission.ToString();
                        });
                });
            }).CreateClient();
        }
    }
}
