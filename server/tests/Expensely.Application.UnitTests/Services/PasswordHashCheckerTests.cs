using Expensely.Application.Abstractions.Cryptography;
using Expensely.Application.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Services
{
    public class PasswordHashCheckerTests
    {
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        public PasswordHashCheckerTests() => _passwordHasherMock = new Mock<IPasswordHasher>();

        [Fact]
        public void Should_call_verify_password_hash_with_provided_passwords()
        {
            var passwordHashChecker = new PasswordHashChecker(_passwordHasherMock.Object);
            const string passwordHash = "password-hash";
            const string providedPassword = "password";

            passwordHashChecker.HashesMatch(passwordHash, providedPassword);

            _passwordHasherMock.Verify(
                x => x.VerifyPasswordHash(It.Is<string>(p => p == passwordHash), It.Is<string>(p => p == providedPassword)),
                Times.Once);
        }

        [Fact]
        public void Should_return_false_if_password_hasher_returns_false()
        {
            _passwordHasherMock.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var passwordHashChecker = new PasswordHashChecker(_passwordHasherMock.Object);

            bool result = passwordHashChecker.HashesMatch(string.Empty, string.Empty);

            result.Should().BeFalse();
        }

        [Fact]
        public void Should_return_true_if_password_hasher_returns_true()
        {
            _passwordHasherMock.Setup(x => x.VerifyPasswordHash(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var passwordHashChecker = new PasswordHashChecker(_passwordHasherMock.Object);

            bool result = passwordHashChecker.HashesMatch(string.Empty, string.Empty);

            result.Should().BeTrue();
        }
    }
}
