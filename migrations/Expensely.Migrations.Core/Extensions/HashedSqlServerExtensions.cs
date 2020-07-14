using System.Reflection;
using DbUp.Builder;
using DbUp.Postgresql;
using Expensely.Migrations.Core.Journal;
using Expensely.Migrations.Core.ScriptProviders;

namespace Expensely.Migrations.Core.Extensions
{
    public static class HashedSqlServerExtensions
    {
        /// <summary>
        /// Configures the upgrade upgrade engine builder with the hashed SQL database.
        /// </summary>
        /// <param name="supportedDatabases">The supported databases.</param>
        /// <param name="connectionManager">The connection manager instance.</param>
        /// <returns>The configured upgrade engine builder instance.</returns>
        public static UpgradeEngineBuilder HashedSqlDatabase(
            this SupportedDatabases supportedDatabases, PostgresqlConnectionManager connectionManager)
        {
            var builder = new UpgradeEngineBuilder();

            builder.Configure(c => c.ConnectionManager = connectionManager);

            builder.Configure(c => c.ScriptExecutor =
                new PostgresqlScriptExecutor(
                    () => c.ConnectionManager,
                    () => c.Log,
                    null,
                    () => c.VariablesEnabled,
                    c.ScriptPreprocessors,
                    () => c.Journal));

            builder.Configure(c => c.Journal =
                new HashedSqlTableJournal(
                    () => c.ConnectionManager,
                    () => c.Log));

            return builder;
        }

        /// <summary>
        /// Adds all scripts found as embedded resources in the given assembly.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="assembly">The assembly.</param>
        /// <param name="journal">The journal.</param>
        /// <returns>The same builder. </returns>
        public static UpgradeEngineBuilder WithHashedScriptsEmbeddedInAssembly(
            this UpgradeEngineBuilder builder, Assembly assembly, HashedSqlTableJournal journal)
        {
            builder.Configure(c => c.ScriptProviders.Add(new HashedEmbeddedScriptsProvider(assembly, journal)));

            return builder;
        }
    }
}
