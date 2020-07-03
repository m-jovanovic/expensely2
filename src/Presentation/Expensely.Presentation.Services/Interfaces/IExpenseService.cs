using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Contracts.Expenses;

namespace Expensely.Presentation.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<IReadOnlyCollection<ExpenseDto>> GetExpensesAsync();
        
        Task RemoveExpenseAsync(Guid expenseId);
    }
}