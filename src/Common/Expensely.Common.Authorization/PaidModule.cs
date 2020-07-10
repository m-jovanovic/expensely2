using System;

namespace Expensely.Common.Authorization
{
    [Flags]
    public enum PaidModule : long
    {
        None = 0,
        LinkedExpenses = 1
    }
}
