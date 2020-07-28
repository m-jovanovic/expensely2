using System;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Xunit;

namespace Expensely.Domain.UnitTests.Entities
{
    public class UserTests
    {
        public static string FirstName = "FirstName";
        public static string LastName = "LastName";
        public static Email Email = Email.Create("test@test.com").Value();

        [Fact]
        public void Should_construct_properly()
        {
            var id = Guid.NewGuid();

            var user = new User(id, FirstName, LastName, Email);

            Assert.NotNull(user);
            Assert.Equal(id, user.Id);
            Assert.Equal(FirstName, user.FirstName);
            Assert.Equal(LastName, user.LastName);
            Assert.Equal(Email, user.Email);
        }

        [Fact]
        public void Should_be_equal_with_user_with_same_id()
        {
            var id = Guid.NewGuid();

            var user1 = new User(id, FirstName, LastName, Email);
            var user2 = new User(id, FirstName, LastName, Email);

            Assert.True(user1.Equals(user2));
            Assert.True(user1 == user2);
            Assert.Equal(user1, user2);
            Assert.Equal(user1.GetHashCode(), user2.GetHashCode());
        }

        [Fact]
        public void Should_not_be_equal_with_user_with_different_id()
        {
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            var user1 = new User(id1, FirstName, LastName, Email);
            var user2 = new User(id2, FirstName, LastName, Email);

            Assert.False(user1.Equals(user2));
            Assert.True(user1 != user2);
            Assert.NotEqual(user1, user2);
            Assert.NotEqual(user1.GetHashCode(), user2.GetHashCode());
        }

        [Fact]
        public void Should_throw_argument_exception_if_id_is_empty()
        {
            Assert.Throws<ArgumentException>(() => new User(Guid.Empty, FirstName, LastName, Email));
        }

        [Fact]
        public void Should_throw_argument_exception_if_first_name_is_empty()
        {
            Assert.Throws<ArgumentException>(() => new User(Guid.NewGuid(), string.Empty, LastName, Email));
        }

        [Fact] public void Should_throw_argument_exception_if_last_name_is_empty()
        {
            Assert.Throws<ArgumentException>(() => new User(Guid.NewGuid(), FirstName, string.Empty, Email));
        }

        [Fact]
        public void Should_throw_argument_exception_if_email_is_empty()
        {
            Assert.Throws<ArgumentException>(() => new User(Guid.NewGuid(), FirstName, LastName, Email.Empty));
        }
    }
}
