using Microsoft.AspNetCore.Builder;

namespace Expensely.Migrations.Core.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Executes all of the migration scripts that have not been ran.
        /// </summary>
        /// <param name="builder">The application builder.</param>
        /// <param name="connectionString">The connection string.</param>
        public static void ExecuteMigrations(this IApplicationBuilder builder, string connectionString)
            => MigrationsManager.ExecuteMigrations(connectionString);
    }
}
