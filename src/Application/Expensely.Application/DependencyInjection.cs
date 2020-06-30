using System.Reflection;
using Expensely.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Expensely.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));
        }
    }
}
