using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Authentication.Commands.Register;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Email;
using Expensely.Domain.ValueObjects;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Authentication.Commands
{
    public class RegisterCommandTests
    {
        private const string FirstName = "FirstName";
        private const string LastName = "LastName";
        private const string Email = "test@test.com";
        private const string Password = "123aA!";
        private const string InvalidConfirmPassword = "123aA!!";
        private static readonly string TokenResponse = Guid.NewGuid().ToString();

        [Fact]
        public void Should_construct_properly()
        {
            var command = new RegisterCommand(FirstName, LastName, Email, Password, Password);

            Assert.NotNull(command);
            Assert.Equal(FirstName, command.FirstName);
            Assert.Equal(LastName, command.LastName);
            Assert.Equal(Email, command.Email);
            Assert.Equal(Password, command.Password);
            Assert.Equal(Password, command.ConfirmPassword);
        }

        [Fact]
        public async Task Handle_should_fail_if_password_and_confirmation_password_do_not_match()
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, Email, Password, InvalidConfirmPassword);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Authentication.PasswordsDoNotMatch, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task Handle_should_fail_if_password_is_null_or_empty(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, Email, password, password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.NullOrEmpty, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("123")]
        [InlineData("1234")]
        [InlineData("12345")]
        public async Task Handle_should_fail_if_password_is_too_short(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, Email, password, password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.TooShort, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("A00001")]
        [InlineData("ABCD0!")]
        [InlineData("A0!!!!")]
        public async Task Handle_should_fail_if_password_is_missing_lowercase_letter(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, Email, password, password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingLowercaseLetter, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("a0000!")]
        [InlineData("abcd0!")]
        [InlineData("a0!!!!")]
        public async Task Handle_should_fail_if_password_is_missing_uppercase_letter(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, Email, password, password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingUppercaseLetter, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("Aaaaa!")]
        public async Task Handle_should_fail_if_password_is_missing_digit(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, Email, password, password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingDigit, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData("A0000a")]
        [InlineData("0Aaaaa")]
        [InlineData("1AAAAb")]
        public async Task Handle_should_fail_if_password_is_missing_non_alphanumeric(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, Email, password, password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Password.MissingNonAlphaNumeric, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task Handle_should_fail_if_email_is_null_or_empty(string email)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, email, Password, Password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.NullOrEmpty, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Fact]
        public async Task Handle_should_fail_email_longer_than_allowed()
        {
            string email = string.Join(
                string.Empty, Enumerable.Range(0, EmailMaxLengthValidator.MaxEmailLength + 1).Select(x => "a"));
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, email, Password, Password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.LongerThanAllowed, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Theory]
        [InlineData(@"NotAnEmail")]
        [InlineData(@"@NotAnEmail")]
        [InlineData(@"""test\test""@example.com")]
        [InlineData("\"test\rtest\"@example.com")]
        [InlineData(@"""test""test""@example.com")]
        [InlineData(@".test@example.com")]
        [InlineData(@"te..st@example.com")]
        [InlineData(@"teeest.@example.com")]
        [InlineData(@".@example.com")]
        [InlineData(@"Tes T@example.com")]
        public async Task Handle_should_fail_if_email_format_is_invalid(string email)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, email, Password, Password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Email.IncorrectFormat, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Fact]
        public async Task Handle_should_call_is_unique_on_user_repository()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, Email, Password, Password);

            await commandHandler.Handle(command, default);

            userRepositoryMock.Verify(x => x.IsUniqueAsync(It.Is<string>(e => e == Email)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_fail_if_email_is_not_unique()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).ReturnsAsync(false);
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                new Mock<IAuthenticationService>().Object);
            var command = new RegisterCommand(FirstName, LastName, Email, Password, Password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal(Errors.Authentication.DuplicateEmail, result.Error);
            Assert.Throws<InvalidOperationException>(() => result.Value());
        }

        [Fact]
        public async Task Handle_should_call_register_on_authentication_service()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                authenticationServiceMock.Object);
            var command = new RegisterCommand(FirstName, LastName, Email, Password, Password);

            await commandHandler.Handle(command, default);

            authenticationServiceMock.Verify(
                x => x.RegisterAsync(
                    It.Is<string>(r => r == FirstName),
                    It.Is<string>(r => r == LastName),
                    It.Is<Email>(r => r == Expensely.Domain.ValueObjects.Email.Create(Email).Value()),
                    It.Is<Password>(r => r == Expensely.Domain.ValueObjects.Password.Create(Password).Value())),
                Times.Once);
        }

        [Fact]
        public async Task Handle_should_succeed_if_command_is_valid()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            authenticationServiceMock.Setup(
                    x => x.RegisterAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Email>(), It.IsAny<Password>()))
                .ReturnsAsync(Result.Ok(new TokenResponse(TokenResponse)));
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                authenticationServiceMock.Object);
            var command = new RegisterCommand(FirstName, LastName, Email, Password, Password);

            Result<TokenResponse> result = await commandHandler.Handle(command, default);

            Assert.False(result.IsFailure);
            Assert.True(result.IsSuccess);
            var tokenResponse = result.Value();
            Assert.NotNull(tokenResponse);
            Assert.Equal(TokenResponse, tokenResponse.Token);
        }
    }
}
