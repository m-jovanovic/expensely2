﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;
using Expensely.Migrations.Hashing;

namespace Expensely.Migrations.Journal
{
    /// <summary>
    /// Represents the hashed SQL table journal.
    /// </summary>
    internal class HashedSqlTableJournal : IJournal
    {
        public const string VersionTableName = "SchemaVersions";
        private const string QuotedVersionTableName = "[SchemaVersions]";
        private const string PrimaryKeyName = "[PK_SchemaVersions_Id]";

        private readonly Func<IConnectionManager> _connectionManager;
        private readonly Func<IUpgradeLog> _log;
        private bool _journalExists;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashedSqlTableJournal"/> class.
        /// </summary>
        /// <param name="connectionManager">The connection manager func.</param>
        /// <param name="log">The upgrade log func.</param>
        internal HashedSqlTableJournal(Func<IConnectionManager> connectionManager, Func<IUpgradeLog> log)
        {
            _connectionManager = connectionManager;
            _log = log;
        }

        /// <inheritdoc />
        public string[] GetExecutedScripts() => Array.Empty<string>();

        /// <inheritdoc />
        public void StoreExecutedScript(SqlScript script, Func<IDbCommand> dbCommandFactory)
        {
            EnsureTableExistsAndIsLatestVersion(dbCommandFactory);

            _connectionManager().ExecuteCommandsWithManagedConnection(_ =>
            {
                StoreExecutedScriptCommand(script, dbCommandFactory);
            });
        }

        /// <inheritdoc />
        public void EnsureTableExistsAndIsLatestVersion(Func<IDbCommand> dbCommandFactory)
        {
            if (_journalExists || (_journalExists = JournalTableExists()))
            {
                return;
            }

            _log().WriteInformation($"Creating the {QuotedVersionTableName} table");

            CreateJournalTableCommand(dbCommandFactory);

            _log().WriteInformation($"The {QuotedVersionTableName} table has been created");

            _journalExists = true;
        }

        /// <summary>
        /// Gets the dictionary containing all of the executed scripts with names as keys and hashes as values.
        /// </summary>
        /// <returns>The dictionary containing all of the executed scripts with names as keys and hashes as values.</returns>
        internal Dictionary<string, string> GetExecutedScriptsDictionary()
        {
            _log().WriteInformation("Fetching list of already executed scripts with their known hash.");

            var scripts = new Dictionary<string, string>();

            _connectionManager().ExecuteCommandsWithManagedConnection(dbCommandFactory =>
            {
                EnsureTableExistsAndIsLatestVersion(dbCommandFactory);

                using IDbCommand command = CreateGetExecutedScriptsCommand(dbCommandFactory);

                using IDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    scripts.Add((string)reader[0], reader[1] == DBNull.Value ? string.Empty : (string)reader[1]);
                }
            });

            return scripts;
        }

        /// <summary>
        /// Stores the specified executed SQL sqlScript.
        /// </summary>
        /// <param name="sqlScript">The SQL sqlScript that was executed.</param>
        /// <param name="dbCommandFactory">The database command factory.</param>
        private static void StoreExecutedScriptCommand(SqlScript sqlScript, Func<IDbCommand> dbCommandFactory)
        {
            using IDbCommand command = dbCommandFactory();

            command.CommandText = $@"
                MERGE {QuotedVersionTableName} AS [Target] 
                USING(SELECT @ScriptName AS ScriptName, @AppliedOnUtc AS AppliedOnUtc, @Hash AS Hash) AS [Source] 
                ON [Target].ScriptName = [Source].ScriptName 
                WHEN MATCHED THEN 
                    UPDATE SET [Target].AppliedOnUtc = [Source].AppliedOnUtc, [Target].Hash = [Source].Hash 
                WHEN NOT MATCHED THEN 
                    INSERT(ScriptName, AppliedOnUtc, Hash) VALUES([Source].ScriptName, [Source].AppliedOnUtc, [Source].Hash);";

            IDbDataParameter scriptNameParameter = command.CreateParameter();
            scriptNameParameter.ParameterName = "ScriptName";
            scriptNameParameter.Value = sqlScript.Name;
            command.Parameters.Add(scriptNameParameter);

            IDbDataParameter appliedParameter = command.CreateParameter();
            appliedParameter.ParameterName = "AppliedOnUtc";
            appliedParameter.Value = DateTime.UtcNow;
            command.Parameters.Add(appliedParameter);

            IDbDataParameter hashParameter = command.CreateParameter();
            hashParameter.ParameterName = "Hash";
            hashParameter.Value = SHA256.ComputeHash(sqlScript.Contents);
            command.Parameters.Add(hashParameter);

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Creates the create journal table.
        /// </summary>
        /// <param name="dbCommandFactory">The database command factory.</param>
        private static void CreateJournalTableCommand(Func<IDbCommand> dbCommandFactory)
        {
            IDbCommand command = dbCommandFactory();

            command.CommandText = $@"
                CREATE TABLE {QuotedVersionTableName}
                (
	                [Id] INT IDENTITY(1,1) NOT NULL CONSTRAINT {PrimaryKeyName} PRIMARY KEY,
	                [ScriptName] VARCHAR(255) NOT NULL,
	                [AppliedOnUtc] DATETIME2(7) NOT NULL,
                    [Hash] VARCHAR(64) NULL
                )";

            command.CommandType = CommandType.Text;

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Create the database command for getting executed scripts.
        /// </summary>
        /// <param name="dbCommandFactory">The database command factory.</param>
        /// <returns>The database command for getting executed scripts.</returns>
        private static IDbCommand CreateGetExecutedScriptsCommand(Func<IDbCommand> dbCommandFactory)
        {
            IDbCommand command = dbCommandFactory();

            command.CommandText = $"SELECT [ScriptName], [Hash] from {QuotedVersionTableName} ORDER BY [ScriptName]";

            command.CommandType = CommandType.Text;

            return command;
        }

        /// <summary>
        /// Checks if the journal table exists.
        /// </summary>
        /// <returns>True if the journal table exists, otherwise false.</returns>
        private bool JournalTableExists()
        {
            return _connectionManager().ExecuteCommandsWithManagedConnection(dbCommandFactory =>
            {
                try
                {
                    using IDbCommand command = dbCommandFactory();

                    command.CommandText = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{VersionTableName}'";

                    command.CommandType = CommandType.Text;

                    var result = (int?)command.ExecuteScalar();

                    return result == 1;
                }
                catch (SqlException)
                {
                    return false;
                }
                catch (DbException)
                {
                    return false;
                }
            });
        }
    }
}