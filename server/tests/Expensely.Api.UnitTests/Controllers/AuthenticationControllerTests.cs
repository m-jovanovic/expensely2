using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Controllers;
using Expensely.Application.Authentication.Commands.Login;
using Expensely.Application.Authentication.Commands.Register;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers
{
    public class AuthenticationControllerTests
    {
        [Fact]
        public async Task Register_should_return_bad_request_if_request_is_null()
        {
            var controller = new AuthenticationController(new Mock<IMediator>().Object);

            IActionResult result = await controller.Register(null);

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Success.Should().BeFalse();
            apiErrorResponse.Errors.Should().Contain(Errors.General.BadRequest);
        }

        [Fact]
        public async Task Register_should_return_bad_request_if_command_returns_failure_result()
        {
            var mediatorMock = new Mock<IMediator>();
            var failureResult = Result.Fail<TokenResponse>(Errors.Authentication.DuplicateEmail);
            mediatorMock.Setup(x => x.Send(It.IsAny<RegisterCommand>(), default)).ReturnsAsync(failureResult);
            var controller = new AuthenticationController(mediatorMock.Object);

            IActionResult result = await controller.Register(CreateRegisterRequest());

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Success.Should().BeFalse();
            apiErrorResponse.Errors.Should().Contain(failureResult.Error);
        }

        [Fact]
        public async Task Register_should_return_ok_if_command_returns_success_result()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<RegisterCommand>(), default))
                .ReturnsAsync(Result.Ok());
            var controller = new AuthenticationController(mediatorMock.Object);

            IActionResult result = await controller.Register(CreateRegisterRequest());

            OkResult okResult = result.As<OkResult>();
            okResult.Should().NotBeNull();
        }

        [Fact]
        public async Task Register_should_send_valid_command()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<RegisterCommand>(), default))
                .ReturnsAsync(Result.Ok(new TokenResponse("Token")));
            var controller = new AuthenticationController(mediatorMock.Object);
            RegisterRequest registerRequest = CreateRegisterRequest();

            await controller.Register(registerRequest);

            mediatorMock.Verify(
                x => x.Send(
                    It.Is<RegisterCommand>(c =>
                        c.FirstName == registerRequest.FirstName &&
                        c.LastName == registerRequest.LastName &&
                        c.Email == registerRequest.Email &&
                        c.Password == registerRequest.Password &&
                        c.ConfirmPassword == registerRequest.ConfirmPassword),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task Login_should_return_bad_request_if_request_is_null()
        {
            var controller = new AuthenticationController(new Mock<IMediator>().Object);

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
            var controller = new AuthenticationController(mediatorMock.Object);

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
                .ReturnsAsync(Result.Ok(new TokenResponse("Token")));
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

        private static RegisterRequest CreateRegisterRequest()
            => new RegisterRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                Password = "Password",
                ConfirmPassword = "Password"
            };

        private static LoginRequest CreateLoginRequest()
            => new LoginRequest
            {
                Email = "Email",
                Password = "Password"
            };
    }
}
