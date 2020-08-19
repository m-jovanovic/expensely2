namespace Expensely.Api.Contracts
{
    /// <summary>
    /// Contains the API endpoint routes.
    /// </summary>
    public static class ApiRoutes
    {
        /// <summary>
        /// Contains the API endpoint routes for transactions.
        /// </summary>
        public static class Transactions
        {
            public const string GetTransactions = "transactions";
        }

        /// <summary>
        /// Contains the API endpoint routes for expenses.
        /// </summary>
        public static class Expenses
        {
            public const string GetExpenses = "expenses";

            public const string GetExpenseById = "expenses/{id:guid}";

            public const string CreateExpense = "expenses";

            public const string DeleteExpense = "expenses/{id:guid}";
        }

        /// <summary>
        /// Contains the API endpoint routes for authentication.
        /// </summary>
        public static class Authentication
        {
            public const string Login = "authentication/login";

            public const string Register = "authentication/register";
        }
    }
}
