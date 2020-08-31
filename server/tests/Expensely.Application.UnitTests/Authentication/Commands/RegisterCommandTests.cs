using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Authentication.Commands.Register;
using Expensely.Application.Core.Abstractions.Cryptography;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Users;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Moq;
using Xunit;
using static Expensely.Tests.Common.Entities.UserData;
using Password = Expensely.Domain.Users.Password;

namespace Expensely.Application.UnitTests.Authentication.Commands
{
    public class RegisterCommandTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task Handle_should_fail_if_password_is_null_or_empty(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Password.NullOrEmpty);
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
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Password.TooShort);
        }

        [Theory]
        [InlineData("A00001")]
        [InlineData("ABCD0!")]
        [InlineData("A0!!!!")]
        public async Task Handle_should_fail_if_password_is_missing_lowercase_letter(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Password.MissingLowercaseLetter);
        }

        [Theory]
        [InlineData("a0000!")]
        [InlineData("abcd0!")]
        [InlineData("a0!!!!")]
        public async Task Handle_should_fail_if_password_is_missing_uppercase_letter(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Password.MissingUppercaseLetter);
        }

        [Theory]
        [InlineData("Aaaaa!")]
        public async Task Handle_should_fail_if_password_is_missing_digit(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Password.MissingDigit);
        }

        [Theory]
        [InlineData("A0000a")]
        [InlineData("0Aaaaa")]
        [InlineData("1AAAAb")]
        public async Task Handle_should_fail_if_password_is_missing_non_alphanumeric(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Password.MissingNonAlphaNumeric);
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
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, email, UserData.Password, UserData.Password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public async Task Handle_should_fail_if_email_is_longer_than_allowed()
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            string email = string.Join(
                string.Empty, Enumerable.Range(0, Email.MaxLength + 1).Select(x => "a"));
            var command = new RegisterCommand(ValidFirstName, ValidLastName, email, UserData.Password, UserData.Password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Email.LongerThanAllowed);
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
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, email, UserData.Password, UserData.Password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Email.InvalidFormat);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task Handle_should_fail_if_first_name_is_null_or_empty(string firstName)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(firstName, ValidLastName, ValidEmail, UserData.Password, UserData.Password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.FirstName.NullOrEmpty);
        }

        [Fact]
        public async Task Handle_should_fail_if_first_name_is_longer_than_allowed()
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            string firstName = string.Join(
                string.Empty, Enumerable.Range(0, FirstName.MaxLength + 1).Select(x => "a"));
            var command = new RegisterCommand(firstName, ValidLastName, ValidEmail, UserData.Password, UserData.Password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.FirstName.LongerThanAllowed);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public async Task Handle_should_fail_if_last_name_is_null_or_empty(string lastName)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(ValidFirstName, lastName, ValidEmail, UserData.Password, UserData.Password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.LastName.NullOrEmpty);
        }

        [Fact]
        public async Task Handle_should_fail_if_last_name_is_longer_than_allowed()
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            string lastName = string.Join(
                string.Empty, Enumerable.Range(0, LastName.MaxLength + 1).Select(x => "a"));
            var command = new RegisterCommand(ValidFirstName, lastName, ValidEmail, UserData.Password, UserData.Password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.LastName.LongerThanAllowed);
        }

        [Fact]
        public async Task Handle_should_call_hash_password_on_password_hasher()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            var passwordHasherMock = new Mock<IPasswordHasher>();
            passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<Password>())).Returns(Guid.NewGuid().ToString);
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                passwordHasherMock.Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, UserData.Password, UserData.Password);

            await commandHandler.Handle(command, default);

            passwordHasherMock.Verify(
                x => x.HashPassword(It.Is<Password>(p => p == Password.Create(UserData.Password).Value())),
                Times.Once);
        }

        [Fact]
        public async Task Handle_should_call_is_unique_on_user_repository()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var passwordHashMock = new Mock<IPasswordHasher>();
            passwordHashMock.Setup(x => x.HashPassword(It.IsAny<Password>())).Returns(Guid.NewGuid().ToString);
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                passwordHashMock.Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, UserData.Password, UserData.Password);

            await commandHandler.Handle(command, default);

            userRepositoryMock.Verify(x => x.IsEmailUniqueAsync(It.Is<string>(e => e == ValidEmail)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_fail_if_email_is_not_unique()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>())).ReturnsAsync(false);
            var passwordHashMock = new Mock<IPasswordHasher>();
            passwordHashMock.Setup(x => x.HashPassword(It.IsAny<Password>())).Returns(Guid.NewGuid().ToString);
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                passwordHashMock.Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, UserData.Password, UserData.Password);

            Result result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Authentication.DuplicateEmail);
        }

        [Fact]
        public async Task Handle_should_call_insert_on_user_repository()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            var passwordHasherMock = new Mock<IPasswordHasher>();
            string passwordHash = Guid.NewGuid().ToString();
            passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<Password>())).Returns(passwordHash);
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                passwordHasherMock.Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, UserData.Password, UserData.Password);

            await commandHandler.Handle(command, default);

            userRepositoryMock.Verify(
                x => x.Insert(It.Is<User>(u =>
                    u.FirstName == ValidFirstName &&
                    u.LastName == ValidLastName &&
                    u.Email.Value == ValidEmail)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_should_succeed_given_valid_command()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.IsEmailUniqueAsync(It.IsAny<string>())).ReturnsAsync(true);
            var passwordHasherMock = new Mock<IPasswordHasher>();
            string passwordHash = Guid.NewGuid().ToString();
            passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<Password>())).Returns(passwordHash);
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                passwordHasherMock.Object);
            var command = new RegisterCommand(ValidFirstName, ValidLastName, ValidEmail, UserData.Password, UserData.Password);

            Result result = await commandHandler.Handle(command, default);

            result.IsSuccess.Should().BeTrue();
        }
    }
}
