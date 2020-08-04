using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using FluentAssertions;
using Xunit;
using static Expensely.Tests.Common.Entities.UserData;

namespace Expensely.Domain.UnitTests.Entities
{
    public class UserTests
    {
        [Fact]
        public void Should_throw_argument_exception_if_id_is_empty()
        {
            Action action = () => new User(Guid.Empty, FirstName, LastName, ValidEmail, PasswordHash);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("id");
        }

        [Fact]
        public void Should_throw_argument_exception_if_first_name_is_empty()
        {
            Action action = () => new User(Guid.NewGuid(), string.Empty, LastName, ValidEmail, PasswordHash);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("firstName");
        }

        [Fact]
        public void Should_throw_argument_exception_if_last_name_is_empty()
        {
            Action action = () => new User(Guid.NewGuid(), FirstName, string.Empty, ValidEmail, PasswordHash);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("lastName");
        }

        [Fact]
        public void Should_throw_argument_exception_if_email_is_empty()
        {
            Action action = () => new User(Guid.NewGuid(), FirstName, LastName, Email.Empty, PasswordHash);

            action.Should().Throw<ArgumentException>().And.ParamName.Should().Be("email");
        }

        [Fact]
        public void Should_construct_properly()
        {
            var id = Guid.NewGuid();

            var user = new User(id, FirstName, LastName, ValidEmail, PasswordHash);

            user.Should().NotBeNull();
            user.Id.Should().Be(id);
            user.FirstName.Should().Be(FirstName);
            user.LastName.Should().Be(LastName);
            user.Email.Should().Be(ValidEmail);
        }

        [Fact]
        public void Should_be_equal_with_user_with_same_id()
        {
            var id = Guid.NewGuid();

            var user1 = new User(id, FirstName, LastName, ValidEmail, PasswordHash);
            var user2 = new User(id, FirstName, LastName, ValidEmail, PasswordHash);

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

            var user1 = new User(id1, FirstName, LastName, ValidEmail, PasswordHash);
            var user2 = new User(id2, FirstName, LastName, ValidEmail, PasswordHash);

            user1.Should().NotBe(user2);
            user2.Should().NotBe(user1);
            (user1 != user2).Should().BeTrue();
            (user2 != user1).Should().BeTrue();
            user1.GetHashCode().Should().NotBe(user2.GetHashCode());
            user2.GetHashCode().Should().NotBe(user1.GetHashCode());
        }
    }
}
