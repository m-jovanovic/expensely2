using System;
using System.Net.Http;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.IntegrationTests.Core;
using Expensely.Application.Contracts.Expenses;
using Expensely.Domain.Authorization;
using Expensely.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace Expensely.Api.IntegrationTests.Controllers.Expenses
{
    public class GetExpenseByIdTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public GetExpenseByIdTests(CustomWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task Should_return_unauthorized_given_unauthorized_user()
        {
            HttpClient client = _factory.CreateClient();

            HttpResponseMessage response = await client.GetAsync(GetExpenseByIdUrl(Guid.NewGuid()));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Should_return_not_found_given_user_with_expense_read_permission_and_invalid_expense_id()
        {
            HttpClient client = _factory.CreateClient(Permission.ExpenseRead);

            HttpResponseMessage response = await client.GetAsync(GetExpenseByIdUrl(Guid.NewGuid()));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        private static string GetExpenseByIdUrl(Guid id) =>
            ApiRoutes.Expenses.GetExpenseById.Replace("{id:guid}", id.ToString());
    }
}
