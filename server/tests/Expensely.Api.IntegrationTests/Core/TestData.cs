using System;

namespace Expensely.Api.IntegrationTests.Core
{
    public static class TestData
    {
        public static readonly Guid UserId = Guid.NewGuid();

        public static Guid ExpenseIdForReading { get; set; }

        public static Guid ExpenseIdForDeleting { get; set; }
    }
}
