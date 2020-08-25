using System;

namespace Expensely.Domain.Authorization
{
    /// <summary>
    /// Represents the paid modules in the application.
    /// </summary>
    [Flags]
    public enum PaidModules : long
    {
        /// <summary>
        /// The default empty module.
        /// </summary>
        None = 0,

        /// <summary>
        /// The linked expenses module.
        /// </summary>
        LinkedExpenses = 1
    }
}
