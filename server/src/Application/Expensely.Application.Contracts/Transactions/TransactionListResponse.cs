using System.Collections.Generic;

namespace Expensely.Application.Contracts.Transactions
{
    /// <summary>
    /// Represents the transaction list response.
    /// </summary>
    public sealed class TransactionListResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionListResponse"/> class.
        /// </summary>
        /// <param name="items">The transactions for the response.</param>
        /// <param name="cursor">The cursor.</param>
        public TransactionListResponse(IReadOnlyCollection<TransactionResponse> items, string cursor = "")
        {
            Items = items;
            Cursor = cursor;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        public IReadOnlyCollection<TransactionResponse> Items { get; }

        /// <summary>
        /// Gets the cursor.
        /// </summary>
        public string Cursor { get; }
    }
}
