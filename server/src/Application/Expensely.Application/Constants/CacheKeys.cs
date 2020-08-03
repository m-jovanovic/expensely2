namespace Expensely.Application.Constants
{
    /// <summary>
    /// Contains the cache keys used in the application.
    /// </summary>
    public static class CacheKeys
    {
        /// <summary>
        /// Contains the cache keys related to expenses.
        /// </summary>
        public static class Expense
        {
            public const string List = "expenses";

            public const string ById = "expenses-{0}";
        }
    }
}
