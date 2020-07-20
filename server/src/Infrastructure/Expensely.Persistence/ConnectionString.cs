namespace Expensely.Persistence
{
    public sealed class ConnectionString
    {
        public const string ExpenselyDb = "ExpenselyDb";

        public ConnectionString(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static implicit operator string(ConnectionString connectionString) => connectionString.Value;
    }
}
