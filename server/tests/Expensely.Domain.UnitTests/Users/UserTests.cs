using System;
using Expensely.Domain.Users;
using Expensely.Domain.Users.Services;
using FluentAssertions;
using Moq;
using Xunit;
using static Expensely.Tests.Common.Entities.UserData;

namespace Expensely.Domain.UnitTests.Users
{
    public class UserTests
    {
        [Fact]
        public void Should_throw_argument_exception_if_id_is_empty()
        {
            Action action = () => new User(Guid.Empty, ValidFirstName, ValidLastName, ValidEmail, PasswordHash);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("id");
        }

        [Fact]
        public void Should_throw_argument_exception_if_first_name_is_empty()
        {
            Action action = () => new User(Guid.NewGuid(), FirstName.Empty, ValidLastName, ValidEmail, PasswordHash);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("firstName");
        }

        [Fact]
        public void Should_throw_argument_exception_if_last_name_is_empty()
        {
            Action action = () => new User(Guid.NewGuid(), ValidFirstName, LastName.Empty, ValidEmail, PasswordHash);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("lastName");
        }

        [Fact]
        public void Should_throw_argument_exception_if_email_is_empty()
        {
            Action action = () => new User(Guid.NewGuid(), ValidFirstName, ValidLastName, Email.Empty, PasswordHash);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("email");
        }

        [Fact]
        public void Should_throw_argument_exception_if_password_hash_is_empty()
        {
            Action action = () => new User(Guid.NewGuid(), ValidFirstName, ValidLastName, ValidEmail, string.Empty);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("passwordHash");
        }

        [Fact]
        public void Should_construct_properly()
        {
            var id = Guid.NewGuid();

            var user = new User(id, ValidFirstName, ValidLastName, ValidEmail, PasswordHash);

            user.Should().NotBeNull();
            user.Id.Should().Be(id);
            user.FirstName.Should().Be(ValidFirstName);
            user.LastName.Should().Be(ValidLastName);
            user.Email.Should().Be(ValidEmail);
        }

        [Fact]
        public void Should_be_equal_with_user_with_same_id()
        {
            var id = Guid.NewGuid();

            var user1 = new User(id, ValidFirstName, ValidLastName, ValidEmail, PasswordHash);
            var user2 = new User(id, ValidFirstName, ValidLastName, ValidEmail, PasswordHash);

            user1.Should().Be(user2);
            user2.Should().Be(user1);
            (user1 == user2).Should().BeTrue();
            (user2 == user1).Should().BeTrue();
            user1.GetHashCode().Should().Be(user2.GetHashCode());
            user2.GetHashCode().Should().Be(user1.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_with_user_with_different_id()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var user1 = new User(id1, ValidFirstName, ValidLastName, ValidEmail, PasswordHash);
            var user2 = new User(id2, ValidFirstName, ValidLastName, ValidEmail, PasswordHash);

            user1.Should().NotBe(user2);
            user2.Should().NotBe(user1);
            (user1 != user2).Should().BeTrue();
            (user2 != user1).Should().BeTrue();
            user1.GetHashCode().Should().NotBe(user2.GetHashCode());
            user2.GetHashCode().Should().NotBe(user1.GetHashCode());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public void Verify_password_hash_should_return_false_if_password_is_null_or_whitespace(string password)
        {
            var user = new User(Guid.NewGuid(), ValidFirstName, ValidLastName, ValidEmail, PasswordHash);

            bool result = user.VerifyPasswordHash(password, new Mock<IPasswordHashChecker>().Object);

            result.Should().BeFalse();
        }

        [Fact]
        public void Verify_password_hash_should_call_hashes_match_with_specified_password()
        {
            var user = new User(Guid.NewGuid(), ValidFirstName, ValidLastName, ValidEmail, PasswordHash);
            var passwordHashCheckerMock = new Mock<IPasswordHashChecker>();

            string password = "password";
            user.VerifyPasswordHash(password, passwordHashCheckerMock.Object);

            passwordHashCheckerMock.Verify(
                x => x.HashesMatch(It.IsAny<string>(), It.Is<string>(p => p == password)),
                Times.Once);
        }

        [Fact]
        public void Verify_password_hash_should_return_false_is_password_hash_checker_returns_false()
        {
            var user = new User(Guid.NewGuid(), ValidFirstName, ValidLastName, ValidEmail, PasswordHash);
            var passwordHashCheckerMock = new Mock<IPasswordHashChecker>();
            passwordHashCheckerMock.Setup(x => x.HashesMatch(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            bool result = user.VerifyPasswordHash("password", passwordHashCheckerMock.Object);

            result.Should().BeFalse();
        }

        [Fact]
        public void Verify_password_hash_should_return_true_is_password_hash_checker_returns_true()
        {
            var user = new User(Guid.NewGuid(), ValidFirstName, ValidLastName, ValidEmail, PasswordHash);
            var passwordHashCheckerMock = new Mock<IPasswordHashChecker>();
            passwordHashCheckerMock.Setup(x => x.HashesMatch(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            bool result = user.VerifyPasswordHash("password", passwordHashCheckerMock.Object);

            result.Should().BeTrue();
        }
    }
}
