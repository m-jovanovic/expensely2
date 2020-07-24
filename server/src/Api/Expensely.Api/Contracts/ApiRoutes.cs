namespace Expensely.Api.Contracts
{
    /// <summary>
    /// Contains the API endpoint routes.
    /// </summary>
    public static class ApiRoutes
    {
        public static class Expenses
        {
            public const string GetExpenses = "api/expenses";

            public const string GetExpenseById = "api/expenses/{id:guid}";

            public const string CreateExpense = "api/expenses";

            public const string DeleteExpense = "api/exepsnes/{id:guid}";
        }

        public static class Authentication
        {
            public const string Login = "api/authentication/login";

            public const string Register = "api/authentication/register";
        }
    }
}
