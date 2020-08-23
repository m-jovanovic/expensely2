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

            // TODO: Use this route when the 'primary-currency' concept is implemented.
            public const string GetCurrentWeekBalanceForPrimaryCurrency = "transactions/balance/current-week/primary";

            public const string GetCurrentWeekBalance = "transactions/balance/current-week/{currencyId:int}";
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
        /// Contains the API endpoint routes for incomes.
        /// </summary>
        public static class Incomes
        {
            public const string GetIncomes = "incomes";

            public const string GetIncomeById = "incomes/{id:guid}";

            public const string CreateIncome = "incomes";

            public const string DeleteIncome = "incomes/{id:guid}";
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
