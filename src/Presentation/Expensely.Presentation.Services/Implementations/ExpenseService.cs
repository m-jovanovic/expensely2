using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Expensely.Common.Contracts.Expenses;
using Expensely.Presentation.Services.Interfaces;

namespace Expensely.Presentation.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly HttpClient _httpClient;

        public ExpenseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<ExpenseDto>> GetExpensesAsync()
        {
            return await _httpClient.GetFromJsonAsync<IReadOnlyCollection<ExpenseDto>>("/api/expenses");
        }

        public async Task RemoveExpenseAsync(Guid expenseId)
        {
            await _httpClient.DeleteAsync($"/api/expenses/{expenseId}");
        }
    }
}