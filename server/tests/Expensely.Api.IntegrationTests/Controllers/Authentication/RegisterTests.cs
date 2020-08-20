using System.Net.Http;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.IntegrationTests.Core;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain.Entities;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Xunit;

namespace Expensely.Api.IntegrationTests.Controllers.Authentication
{
    public class RegisterTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public RegisterTests(CustomWebApplicationFactory factory) => _client = factory.CreateClient();

        [Fact]
        public async Task Should_return_bad_request_given_empty_request()
        {
            RegisterRequest? request = null;

            HttpResponseMessage response = await _client.PostAsync(ApiRoutes.Authentication.Register, new JsonContent(request));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Should_return_bad_request_given_invalid_request()
        {
            RegisterRequest request = new RegisterRequest
            {
                FirstName = UserData.ValidFirstName,
                LastName = UserData.ValidLastName,
                Email = UserData.ValidEmail,
                Password = string.Empty
            };

            HttpResponseMessage response = await _client.PostAsync(ApiRoutes.Authentication.Register, new JsonContent(request));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task Should_return_ok_given_valid_request()
        {
            RegisterRequest request = new RegisterRequest
            {
                FirstName = UserData.ValidFirstName,
                LastName = UserData.ValidLastName,
                Email = "test-valid@expensely.net",
                Password = UserData.Password,
                ConfirmPassword = UserData.Password
            };

            HttpResponseMessage response = await _client.PostAsync(ApiRoutes.Authentication.Register, new JsonContent(request));

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
