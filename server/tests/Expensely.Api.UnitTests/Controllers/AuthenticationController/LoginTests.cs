using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Application.Authentication.Commands.Login;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.AuthenticationController
{
    public class LoginTests
    {
        [Fact]
        public async Task Login_should_return_bad_request_if_request_is_null()
        {
            var controller = new Api.Controllers.AuthenticationController(new Mock<IMediator>().Object);

            IActionResult result = await controller.Login(null);

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Success.Should().BeFalse();
            apiErrorResponse.Errors.Should().Contain(Errors.General.BadRequest);
        }

        [Fact]
        public async Task Login_should_return_bad_request_if_command_returns_failure_result()
        {
            var mediatorMock = new Mock<IMediator>();
            var failureResult = Result.Fail<TokenResponse>(Errors.Authentication.UserNotFound);
            mediatorMock.Setup(x => x.Send(It.IsAny<LoginCommand>(), default)).ReturnsAsync(failureResult);
            var controller = new Api.Controllers.AuthenticationController(mediatorMock.Object);

            IActionResult result = await controller.Login(CreateLoginRequest());

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Success.Should().BeFalse();
            apiErrorResponse.Errors.Should().Contain(failureResult.Error);
        }

        [Fact]
        public async Task Login_should_return_ok_if_command_returns_success_result()
        {
            var mediatorMock = new Mock<IMediator>();
            const string tokenValue = "Token";
            mediatorMock.Setup(x => x.Send(It.IsAny<LoginCommand>(), default))
                .ReturnsAsync(Result.Ok(new TokenResponse("Token")));
            var controller = new Api.Controllers.AuthenticationController(mediatorMock.Object);

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
                .ReturnsAsync(Result.Ok(new TokenResponse("Token")));
            var controller = new Api.Controllers.AuthenticationController(mediatorMock.Object);
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
