﻿using System;
using System.Threading.Tasks;
using Expensely.Domain.Entities;
using Expensely.Infrastructure.Persistence.IntegrationTests.Common;
using Expensely.Infrastructure.Persistence.Repositories;
using FluentAssertions;
using Xunit;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.Repositories
{
    public class UserRepositoryTests : DbContextTest
    {
        private const string Email = "test@expesnely.com";
        private const string UniqueEmail = "test1@expensley.com";

        [Fact]
        public async Task Is_unique_should_return_true_if_no_users_exist()
        {
            var userRepository = new UserRepository(DbContext);

            bool isUnique = await userRepository.IsUniqueAsync(Email);

            isUnique.Should().BeTrue();
        }

        [Fact]
        public async Task Is_unique_should_return_true_if_user_with_email_does_not_exist()
        {
            await SeedUser(UniqueEmail);
            var userRepository = new UserRepository(DbContext);

            bool isUnique = await userRepository.IsUniqueAsync(Email);

            isUnique.Should().BeTrue();
        }

        [Fact]
        public async Task Is_unique_should_return_false_if_user_with_email_exists()
        {
            await SeedUser(Email);
            var userRepository = new UserRepository(DbContext);

            bool isUnique = await userRepository.IsUniqueAsync(Email);

            isUnique.Should().BeFalse();
        }

        private async Task SeedUser(string email)
        {
            var user = new User(
                Guid.NewGuid(),
                "FirstName",
                "LastName",
                Domain.ValueObjects.Email.Create(email).Value(),
                Guid.NewGuid().ToString());

            DbContext.Add(user);

            await DbContext.SaveChangesAsync();
        }
    }
}
