using System.Net.Http;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.IntegrationTests.Core;
using Expensely.Application.Contracts.Authentication;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace Expensely.Api.IntegrationTests.Controllers.Authentication
{
    public class LoginTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public LoginTests(CustomWebApplicationFactory factory) => _client = factory.CreateClient();

        [Fact]
        public async Task Should_return_bad_request_given_empty_request()
        {
            LoginRequest? request = null;

            HttpResponseMessage response = await _client.PostAsync(ApiRoutes.Authentication.Login, new JsonContent(request));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Should_return_bad_request_given_invalid_credentials()
        {
            LoginRequest request = new LoginRequest
            {
                Email = UserData.ValidEmail,
                Password = "invalid-password"
            };

            HttpResponseMessage response = await _client.PostAsync(ApiRoutes.Authentication.Login, new JsonContent(request));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Should_return_ok_given_valid_credentials()
        {
            LoginRequest request = new LoginRequest
            {
                Email = UserData.ValidEmail,
                Password = UserData.Password
            };

            HttpResponseMessage response = await _client.PostAsync(ApiRoutes.Authentication.Login, new JsonContent(request));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
            string content = await response.Content.ReadAsStringAsync();
            TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);
            tokenResponse.Should().NotBeNull();
            tokenResponse.Token.Should().NotBeEmpty();
        }
    }
}
