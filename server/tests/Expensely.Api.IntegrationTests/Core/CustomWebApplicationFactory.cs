using System;
using System.Linq;
using System.Net.Http;
using Expensely.Domain.Enums;
using Expensely.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
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

            builder.ConfigureServices(services =>
            {
                ServiceDescriptor descriptor = services
                    .SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ExpenselyDbContext>));

                services.Remove(descriptor);
                
                services.AddDbContext<ExpenselyDbContext>(options =>
                {
                    options.UseInMemoryDatabase("Expensely");
                });

                ServiceProvider sp = services.BuildServiceProvider();

                using IServiceScope serviceScope = sp.CreateScope();

                IServiceProvider scopedServices = serviceScope.ServiceProvider;

                using ExpenselyDbContext dbContext = scopedServices.GetRequiredService<ExpenselyDbContext>();

                ILogger<CustomWebApplicationFactory> logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                dbContext.Database.EnsureCreated();

                try
                {
                    DatabaseInitializer.InitializeExpensesForTestsAsync(dbContext).GetAwaiter().GetResult();
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
