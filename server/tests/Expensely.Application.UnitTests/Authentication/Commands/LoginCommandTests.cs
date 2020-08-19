using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Cryptography;
using Expensely.Application.Abstractions.Repositories;
using Expensely.Application.Authentication.Commands.Login;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Moq;
using Xunit;
using static Expensely.Tests.Common.Commands.Authentication.LoginCommandData;

namespace Expensely.Application.UnitTests.Authentication.Commands
{
    public class LoginCommandTests
    {
        [Fact]
        public async Task Handle_call_get_by_email_on_user_repository()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var commandHandler = new LoginCommandHandler(
                userRepositoryMock.Object,
                new Mock<IPasswordHasher>().Object,
                new Mock<IJwtProvider>().Object);
            var command = CreateValidCommand();

            await commandHandler.Handle(command, default);

            userRepositoryMock.Verify(x => x.GetByEmailAsync(It.Is<string>(e => e == command.Email)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_fail_if_user_with_email_is_not_found()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
            var commandHandler = new LoginCommandHandler(
                userRepositoryMock.Object,
                new Mock<IPasswordHasher>().Object,
                new Mock<IJwtProvider>().Object);
            var command = CreateValidCommand();

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Authentication.InvalidEmailOrPassword);
        }

        [Fact]
        public async Task Handle_should_call_verify_password_on_password_hasher()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            User user = UserData.CreateUser();
            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            var passwordHasherMock = new Mock<IPasswordHasher>();
            var commandHandler = new LoginCommandHandler(
                userRepositoryMock.Object,
                passwordHasherMock.Object,
                new Mock<IJwtProvider>().Object);
            var command = CreateValidCommand();

            await commandHandler.Handle(command, default);

            passwordHasherMock.Verify(x =>
                x.VerifyHashedPassword(It.Is<string>(p => p == user.PasswordHash), It.Is<string>(p => p == command.Password)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_should_fail_if_password_verification_fails()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            User user = UserData.CreateUser();
            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Failure);
            var commandHandler = new LoginCommandHandler(
                userRepositoryMock.Object,
                passwordHasherMock.Object,
                new Mock<IJwtProvider>().Object);
            var command = CreateValidCommand();

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Authentication.InvalidEmailOrPassword);
        }

        [Fact]
        public async Task Handle_should_call_create_on_jwt_provider()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            User user = UserData.CreateUser();
            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);
            var jwtProvider = new Mock<IJwtProvider>();
            var commandHandler = new LoginCommandHandler(
                userRepositoryMock.Object,
                passwordHasherMock.Object,
                jwtProvider.Object);
            var command = CreateValidCommand();

            await commandHandler.Handle(command, default);

            jwtProvider.Verify(x => x.CreateAsync(It.Is<User>(u => u == user)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_succeed_given_valid_command()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            User user = UserData.CreateUser();
            userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(x => x.VerifyHashedPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);
            var jwtProvider = new Mock<IJwtProvider>();
            var tokenResponse = new TokenResponse("Token response");
            jwtProvider.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(tokenResponse);
            var commandHandler = new LoginCommandHandler(
                userRepositoryMock.Object,
                passwordHasherMock.Object,
                jwtProvider.Object);
            var command = CreateValidCommand();

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            result.Value().Should().BeEquivalentTo(tokenResponse);
        }
    }
}
