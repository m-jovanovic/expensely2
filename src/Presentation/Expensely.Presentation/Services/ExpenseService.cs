using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Expensely.Contracts.Expenses;

namespace Expensely.Presentation.Services
{
    public class ExpenseService
    {
        private readonly HttpClient _httpClient;

        public ExpenseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<ExpenseDto>> GetExpenses()
        {
            return await _httpClient.GetFromJsonAsync<IReadOnlyCollection<ExpenseDto>>("/api/expenses");
        }
    }
}
