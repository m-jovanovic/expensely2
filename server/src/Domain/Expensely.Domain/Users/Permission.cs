using System.ComponentModel.DataAnnotations;
using Expensely.Domain.Users.Attributes;

namespace Expensely.Domain.Users
{
    /// <summary>
    /// Represents the application permissions.
    /// </summary>
    public enum Permission
    {
        [Display(GroupName = "Transaction", Name = "Read", Description = "Can read transactions.")]
        TransactionRead = 0,

        [Display(GroupName = "Expense", Name = "Read", Description = "Can read expenses.")]
        ExpenseRead = 10,

        [Display(GroupName = "Expense", Name = "Create", Description = "Can create expenses.")]
        ExpenseCreate = 11,

        [Display(GroupName = "Expense", Name = "Update", Description = "Can update expenses.")]
        ExpenseUpdate = 12,

        [Display(GroupName = "Expense", Name = "Remove", Description = "Can remove expenses.")]
        ExpenseDelete = 13,

        [LinkedToModule(PaidModules.LinkedExpenses)]
        [Display(GroupName = "Features", Name = "Linked expenses", Description = "Can access linked expenses feature.")]
        LinkedExpensesAccess = 1000,

        [Display(GroupName = "Admin", Name = "Access everything", Description = "Allow access to every feature.")]
        AccessEverything = int.MaxValue
    }
}
