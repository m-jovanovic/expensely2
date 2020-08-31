using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Controllers;
using Expensely.Application.Authentication.Commands.Register;
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
    public class RegisterTests
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
            apiErrorResponse.Errors.Should().Contain(Errors.General.BadRequest);
        }

        [Fact]
        public async Task Register_should_return_bad_request_if_command_returns_failure_result()
        {
            var mediatorMock = new Mock<IMediator>();
            var failureResult = Result.Failure<TokenResponse>(Errors.Authentication.DuplicateEmail);
            mediatorMock.Setup(x => x.Send(It.IsAny<RegisterCommand>(), default)).ReturnsAsync(failureResult);
            var controller = new AuthenticationController(mediatorMock.Object);

            IActionResult result = await controller.Register(CreateRegisterRequest());

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Errors.Should().Contain(failureResult.Error);
        }

        [Fact]
        public async Task Register_should_return_ok_if_command_returns_success_result()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<RegisterCommand>(), default))
                .ReturnsAsync(Result.Success());
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
                .ReturnsAsync(Result.Success(new TokenResponse("Token")));
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

        private static RegisterRequest CreateRegisterRequest()
            => new RegisterRequest
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Email = "Email",
                Password = "Password",
                ConfirmPassword = "Password"
            };
    }
}
