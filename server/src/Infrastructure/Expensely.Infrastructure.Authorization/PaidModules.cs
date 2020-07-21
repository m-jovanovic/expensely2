using System;

namespace Expensely.Infrastructure.Authorization
{
    [Flags]
    public enum PaidModules : long
    {
        None = 0,
        LinkedExpenses = 1
    }
}
