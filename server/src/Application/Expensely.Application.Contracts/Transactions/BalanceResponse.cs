using Expensely.Domain.Transactions;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the balance response.
    /// </summary>
    public sealed class BalanceResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BalanceResponse"/> class.
        /// </summary>
        /// <param name="currencyId">The currency identifier.</param>
        /// <param name="amount">The amount.</param>
        public BalanceResponse(int currencyId, decimal amount)
        {
            Amount = amount;
            Currency currency = Currency.FromValue(currencyId);
            Balance = currency.Format(amount);
        }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the balance.
        /// </summary>
        public string Balance { get; }
    }
}
