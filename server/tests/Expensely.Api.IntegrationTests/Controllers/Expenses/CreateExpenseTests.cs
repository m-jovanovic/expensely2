using System;
using System.Net.Http;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.IntegrationTests.Core;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Expenses;
using Expensely.Domain.Authorization;
using Expensely.Domain.Transactions;
using Expensely.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace Expensely.Api.IntegrationTests.Controllers.Expenses
{
    public class CreateExpenseTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CreateExpenseTests(CustomWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task Should_return_unauthorized_given_unauthorized_user()
        {
            HttpClient client = _factory.CreateClient();

            HttpResponseMessage response = await client.PostAsync(
                ApiRoutes.Expenses.CreateExpense,
                new StringContent(string.Empty));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Should_return_bad_request_given_user_with_expense_create_permission_and_invalid_request()
        {
            HttpClient client = _factory.CreateClient(Permission.ExpenseCreate);
            var request = new CreateExpenseRequest
            {
                Name = "Shopping",
                Amount = 100.0m,
                CurrencyId = default,
                Date = DateTime.Now
            };
            
            HttpResponseMessage response = await client.PostAsync(ApiRoutes.Expenses.CreateExpense, new JsonContent(request));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Should_return_created_given_user_with_expense_create_and_valid_request()
        {
            HttpClient client = _factory.CreateClient(Permission.ExpenseCreate);
            var request = new CreateExpenseRequest
            {
                Name = "Shopping",
                Amount = -100.0m,
                CurrencyId = Currency.Usd.Value,
                Date = DateTime.Now
            };
            
            HttpResponseMessage response = await client.PostAsync(ApiRoutes.Expenses.CreateExpense, new JsonContent(request));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status201Created);
            string content = await response.Content.ReadAsStringAsync();
            EntityCreatedResponse entityCreatedResponse = JsonConvert.DeserializeObject<EntityCreatedResponse>(content);
            entityCreatedResponse.Should().NotBeNull();
            entityCreatedResponse.Id.Should().NotBeEmpty();
            response.Headers.Location.AbsoluteUri.Should()
                .Contain(ApiRoutes.Expenses.GetExpenseById.Replace("{id:guid}", entityCreatedResponse.Id.ToString()));
        }
    }
}
