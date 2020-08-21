namespace Expensely.Application.Core.Constants
{
    /// <summary>
    /// Contains the cache keys used in the application.
    /// </summary>
    public static class CacheKeys
    {
        /// <summary>
        /// Contains the cache keys related to transactions.
        /// </summary>
        public static class Transactions
        {
            /// <summary>
            /// The cache key prefix for transaction cache keys.
            /// </summary>
            /// <remarks>
            /// {0} - User identifier.
            /// </remarks>
            public const string CacheKeyPrefix = "transactions-{0}";

            /// <summary>
            /// The transactions list cache key.
            /// </summary>
            /// <remarks>
            /// {0} - User identifier.
            /// {1] - Limit.
            /// {2} - Cursor.
            /// </remarks>
            public const string TransactionsList = "transactions-{0}-list-{1}-{2}";
        }

        /// <summary>
        /// Contains the cache keys related to expenses.
        /// </summary>
        public static class Expenses
        {
            /// <summary>
            /// The cache key prefix for expense cache keys.
            /// </summary>
            /// <remarks>
            /// {0} - User identifier.
            /// </remarks>
            public const string CacheKeyPrefix = "expenses-{0}";

            /// <summary>
            /// The expenses list cache key.
            /// </summary>
            /// <remarks>
            /// {0} - User identifier.
            /// {1] - Limit.
            /// {2} - Cursor.
            /// </remarks>
            public const string ExpensesList = "expenses-{0}-list-{1}-{2}";

            /// <summary>
            /// The expense by identifier cache key.
            /// </summary>
            /// <remarks>
            /// {0} - User identifier.
            /// {1] - Expense identifier.
            /// </remarks>
            public const string ExpenseById = "expenses-{0}-by-id-{1}";
        }

        /// <summary>
        /// Contains the cache keys related to incomes.
        /// </summary>
        public static class Incomes
        {
            /// <summary>
            /// The cache key prefix for income cache keys.
            /// </summary>
            /// <remarks>
            /// {0} - User identifier.
            /// </remarks>
            public const string CacheKeyPrefix = "incomes-{0}";

            /// <summary>
            /// The incomes list cache key.
            /// </summary>
            /// <remarks>
            /// {0} - User identifier.
            /// {1] - Limit.
            /// {2} - Cursor.
            /// </remarks>
            public const string IncomesList = "incomes-{0}-list-{1}-{2}";

            /// <summary>
            /// The incomes by identifier cache key.
            /// </summary>
            /// <remarks>
            /// {0} - User identifier.
            /// {1] - Income identifier.
            /// </remarks>
            public const string IncomeById = "incomes-{0}-by-id-{1}";
        }
    }
}
