using System;
using System.Reflection;
using DbUp;
using DbUp.Engine;

namespace Expensely.Migrations.Core
{
    public static class MigrationManager
    {
        public static (bool Success, Exception? Error) ExecuteMigrations(string connectionString)
        {
            UpgradeEngine upgradeEngine = DeployChanges
                .To
                .SqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            EnsureDatabase.For.SqlDatabase(connectionString);

            DatabaseUpgradeResult result = upgradeEngine.PerformUpgrade();

            return (result.Successful, result.Error);
        }
    }
}
