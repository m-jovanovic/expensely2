using System;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Expensely.Api;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Primitives;
using Expensely.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Expensely.Application.IntegrationTests.Common
{
    public static class Testing
    {
        private const string DatabaseName = "Expensely";
        private static readonly IConfiguration Configuration;
        private static readonly IServiceScopeFactory ScopeFactory;
        public static readonly Guid UserId = Guid.NewGuid();

        static Testing()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            var services = new ServiceCollection();
            
            var startup = new Startup(Configuration);

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(
                webHostEnvironment => webHostEnvironment.ApplicationName == "Expensely.Api" &&
                                      webHostEnvironment.EnvironmentName == "Development"));

            startup.ConfigureServices(services);

            services.AddScoped(factory =>
            {
                var userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();

                userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(UserId);

                return userIdentifierProviderMock.Object;
            });

            services.RemoveAll(typeof(ExpenselyDbContext));

            services.AddDbContext<ExpenselyDbContext>(options =>
                options.UseInMemoryDatabase(DatabaseName));

            ScopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>()
                ?? throw new ArgumentNullException();
        }

        public static async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : Entity
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ExpenselyDbContext>()!;

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        public static async Task<TEntity?> FindAsync<TEntity>(Guid id)
            where TEntity : Entity
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ExpenselyDbContext>()!;

            return await context.FindAsync<TEntity>(id);
        }

        public static async Task<TEntity?> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : Entity
        {
            using var scope = ScopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ExpenselyDbContext>()!;

            return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = ScopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>()!;

            return await mediator.Send(request);
        }
    }
}
