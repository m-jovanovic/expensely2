using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.IntegrationTests.Core;
using Expensely.Application.Contracts.Expenses;
using Expensely.Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace Expensely.Api.IntegrationTests.Controllers.ExpensesController
{
    public class GetExpensesTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetExpensesTests(CustomWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task Should_return_unauthorized_given_unauthorized_user()
        {
            HttpClient client = _factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync(ApiRoutes.Expenses.GetExpenses);

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Should_return_ok_given_user_with_expense_read_permission()
        {
            HttpClient client = _factory.CreateClient(Permission.ExpenseRead);

            HttpResponseMessage response = await client.GetAsync(ApiRoutes.Expenses.GetExpenses);

            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            string content = await response.Content.ReadAsStringAsync();
            IReadOnlyCollection<ExpenseResponse> expenses =
                JsonConvert.DeserializeObject<IReadOnlyCollection<ExpenseResponse>>(content);
            expenses.Should().NotBeNull();
            expenses.Should().NotBeEmpty();
            expenses.Should().NotContainNulls();
        }
    }
}
