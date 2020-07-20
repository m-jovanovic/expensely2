using System;
using Expensely.Migrations.Core;
using Microsoft.Extensions.Configuration;

namespace Expensely.Migrations.Runner
{
    internal static class Program
    {
        private const string AppSettingsJsonFileName = "appsettings.json";
        private const string ConnectionStringName = "ExpenselyDb";

        internal static int Main()
        {
            IConfigurationRoot configurationRoot = BuildConfigurationRoot();

            string connectionString = configurationRoot.GetConnectionString(ConnectionStringName);

            (bool success, Exception? error) = MigrationsManager.ExecuteMigrations(connectionString);

            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("The migrations have been executed successfully.");

                Console.ResetColor();

                return 0;
            }

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("The migrations execution encountered an error.");

            Console.WriteLine(error);

            Console.ResetColor();

            return -1;
        }

        private static IConfigurationRoot BuildConfigurationRoot()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(AppSettingsJsonFileName);

            return configurationBuilder.Build();
        }
    }
}
