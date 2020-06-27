using System;
using Expensely.Migrations.Core;

namespace Expensely.Migrations.Runner
{
    internal static class Program
    {
        internal static int Main()
        {
            // TODO: Pull connection string from configuration.
            string connectionString = string.Empty;

            (bool success, Exception? error) = MigrationManager.ExecuteMigrations(connectionString);

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

            #if DEBUG
            Console.ReadLine();
            #endif

            return -1;
        }
    }
}
