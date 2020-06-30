namespace Expensely.Persistence
{
    internal sealed class ConnectionString
    {
        internal ConnectionString(string value)
        {
            Value = value;
        }

        internal string Value { get; }

        public static implicit operator string(ConnectionString connectionString) => connectionString.Value;
    }
}
