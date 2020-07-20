using System;

namespace Expensely.Common.Authorization
{
    [Flags]
    public enum PaidModules : long
    {
        None = 0,
        LinkedExpenses = 1
    }
}
