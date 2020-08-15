using System.Net.Http;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.IntegrationTests.Core;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace Expensely.Api.IntegrationTests.Controllers.Expenses
{
    public class GetExpensesTests : IClassFixture<CustomWebApplicationFactory>
    {
        private const int Limit = 1;
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
    }
}
