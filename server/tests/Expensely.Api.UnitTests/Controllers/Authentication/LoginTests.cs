using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Controllers;
using Expensely.Application.Authentication.Commands.Login;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain;
using Expensely.Domain.Core;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Authentication
{
    public class LoginTests
    {
        [Fact]
        public async Task Login_should_return_bad_request_if_request_is_null()
        {
            var controller = new AuthenticationController(new Mock<IMediator>().Object);

            IActionResult result = await controller.Login(null);

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Errors.Should().Contain(Errors.General.BadRequest);
        }

        [Fact]
        public async Task Login_should_return_bad_request_if_command_returns_failure_result()
        {
            var mediatorMock = new Mock<IMediator>();
            var failureResult = Result.Failure<TokenResponse>(Errors.Authentication.InvalidEmailOrPassword);
            mediatorMock.Setup(x => x.Send(It.IsAny<LoginCommand>(), default)).ReturnsAsync(failureResult);
            var controller = new AuthenticationController(mediatorMock.Object);

            IActionResult result = await controller.Login(CreateLoginRequest());

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Errors.Should().Contain(failureResult.Error);
        }

        [Fact]
        public async Task Login_should_return_ok_if_command_returns_success_result()
        {
            var mediatorMock = new Mock<IMediator>();
            const string tokenValue = "Token";
            mediatorMock.Setup(x => x.Send(It.IsAny<LoginCommand>(), default))
                .ReturnsAsync(Result.Success(new TokenResponse("Token")));
            var controller = new AuthenticationController(mediatorMock.Object);

            IActionResult result = await controller.Login(CreateLoginRequest());

            OkObjectResult okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            TokenResponse tokenResponse = okObjectResult.Value.As<TokenResponse>();
            tokenResponse.Should().NotBeNull();
            tokenResponse.Token.Should().Be(tokenValue);
        }

        [Fact]
        public async Task Login_should_send_valid_command()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<LoginCommand>(), default))
                .ReturnsAsync(Result.Success(new TokenResponse("Token")));
            var controller = new AuthenticationController(mediatorMock.Object);
            LoginRequest loginRequest = CreateLoginRequest();

            await controller.Login(loginRequest);

            mediatorMock.Verify(
                x => x.Send(
                    It.Is<LoginCommand>(c =>
                        c.Email == loginRequest.Email &&
                        c.Password == loginRequest.Password),
                    default),
                Times.Once);
        }

        private static LoginRequest CreateLoginRequest()
            => new LoginRequest
            {
                Email = "Email",
                Password = "Password"
            };
    }
}
