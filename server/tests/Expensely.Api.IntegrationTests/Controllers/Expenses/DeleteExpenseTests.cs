using System;
using System.Net.Http;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.IntegrationTests.Core;
using Expensely.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Expensely.Api.IntegrationTests.Controllers.Expenses
{
    public class DeleteExpenseTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public DeleteExpenseTests(CustomWebApplicationFactory factory) => _factory = factory;

        [Fact]
        public async Task Should_return_unauthorized_given_unauthorized_user()
        {
            HttpClient client = _factory.CreateClient();

            HttpResponseMessage response = await client.DeleteAsync(DeleteExpenseUrl(Guid.NewGuid()));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        [Fact]
        public async Task Should_return_not_found_given_user_with_expense_delete_permission_and_invalid_expense_id()
        {
            HttpClient client = _factory.CreateClient(Permission.ExpenseDelete);

            HttpResponseMessage response = await client.DeleteAsync(DeleteExpenseUrl(Guid.NewGuid()));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Should_return_no_content_given_user_with_expense_delete_permission_and_valid_expense_id()
        {
            HttpClient client = _factory.CreateClient(Permission.ExpenseDelete);

            HttpResponseMessage response = await client.DeleteAsync(DeleteExpenseUrl(TestData.ExpenseIdForDeleting));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        private static string DeleteExpenseUrl(Guid id) =>
            ApiRoutes.Expenses.DeleteExpense.Replace("{id:guid}", id.ToString());
    }
}
