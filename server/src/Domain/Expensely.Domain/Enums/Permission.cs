using System.ComponentModel.DataAnnotations;
using Expensely.Domain.Attributes;

namespace Expensely.Domain.Enums
{
    /// <summary>
    /// Represents the application permissions.
    /// </summary>
    public enum Permission
    {
        [Display(GroupName = "Expense", Name = "Read", Description = "Can read expenses.")]
        ExpenseRead = 0,

        [Display(GroupName = "Expense", Name = "Create", Description = "Can create expenses.")]
        ExpenseCreate = 1,

        [Display(GroupName = "Expense", Name = "Update", Description = "Can update expenses.")]
        ExpenseUpdate = 2,

        [Display(GroupName = "Expense", Name = "Remove", Description = "Can remove expenses.")]
        ExpenseRemove = 3,

        [LinkedToModule(PaidModules.LinkedExpenses)]
        [Display(GroupName = "Features", Name = "Linked expenses", Description = "Can access linked expenses feature.")]
        LinkedExpensesAccess = 1000,

        [Display(GroupName = "Admin", Name = "Access everything", Description = "Allow access to every feature.")]
        AccessEverything = int.MaxValue
    }
}
