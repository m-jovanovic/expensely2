using System;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Domain.Transactions
{
    /// <summary>
    /// Represents the category of the transaction.
    /// </summary>
    public sealed class Category : Enumeration<Category>
    {
        public static readonly Category Food = new Category(1, "Food");
        public static readonly Category Shopping = new Category(2, "Shopping", Food);

        private Category(int value, string name)
            : base(value, name)
        {
            SubCategories = Array.Empty<Category>();
        }

        private Category(int value, string name, params Category[] categories)
            : base(value, name)
        {
            SubCategories = categories;
        }

        private Category()
        {
            SubCategories = Array.Empty<Category>();
        }

        public Category[] SubCategories { get; }
    }
}
