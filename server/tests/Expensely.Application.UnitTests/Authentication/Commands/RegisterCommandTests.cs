﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Cryptography;
using Expensely.Application.Abstractions.Repositories;
using Expensely.Application.Authentication.Commands.Register;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Validators.Email;
using FluentAssertions;
using Moq;
using Xunit;
using static Expensely.Tests.Common.Entities.UserData;

namespace Expensely.Application.UnitTests.Authentication.Commands
{
    public class RegisterCommandTests
    {
        private const string InvalidConfirmPassword = "123aA!!";
        private static readonly string TokenResponse = Guid.NewGuid().ToString();


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
            var command = new RegisterCommand(FirstName, LastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
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
            var command = new RegisterCommand(FirstName, LastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
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
            var command = new RegisterCommand(FirstName, LastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
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
            var command = new RegisterCommand(FirstName, LastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Password.MissingUppercaseLetter);
        }

        [Theory]
        [InlineData("Aaaaa!")]
        public async Task Handle_should_fail_if_password_is_missing_digit(string password)
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(FirstName, LastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
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
            var command = new RegisterCommand(FirstName, LastName, ValidEmail, password, password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
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
            var command = new RegisterCommand(FirstName, LastName, email, Password, Password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Email.NullOrEmpty);
        }

        [Fact]
        public async Task Handle_should_fail_email_longer_than_allowed()
        {
            var commandHandler = new RegisterCommandHandler(
                new Mock<IUserRepository>().Object,
                new Mock<IPasswordHasher>().Object);
            string email = string.Join(
                string.Empty, Enumerable.Range(0, EmailMaxLengthValidator.MaxEmailLength + 1).Select(x => "a"));
            var command = new RegisterCommand(FirstName, LastName, email, Password, Password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
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
            var command = new RegisterCommand(FirstName, LastName, email, Password, Password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Email.IncorrectFormat);
        }

        [Fact]
        public async Task Handle_should_call_is_unique_on_user_repository()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(FirstName, LastName, ValidEmail, Password, Password);

            await commandHandler.Handle(command, default);

            userRepositoryMock.Verify(x => x.IsUniqueAsync(It.Is<string>(e => e == ValidEmail)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_fail_if_email_is_not_unique()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.IsUniqueAsync(It.IsAny<string>())).ReturnsAsync(false);
            var commandHandler = new RegisterCommandHandler(
                userRepositoryMock.Object,
                new Mock<IPasswordHasher>().Object);
            var command = new RegisterCommand(FirstName, LastName, ValidEmail, Password, Password);

            Result result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Authentication.DuplicateEmail);
        }
    }
}
