using System;
using System.Threading.Tasks;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;
using Expensely.Infrastructure.Persistence.IntegrationTests.Common;
using FluentAssertions;
using Xunit;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.DbContext
{
    public class AuditableEntitiesTests : DbContextTest
    {
        [Fact]
        public async Task Should_set_created_on_date_when_adding_new_expense()
        {
            var expense = CreateExpense();

            await AddEntity(expense);

            expense.CreatedOnUtc.Should().NotBe(default);
            expense.ModifiedOnUtc.Should().BeNull();
        }

        [Fact]
        public async Task Should_set_modified_on_date_when_updating_existing_expense()
        {
            var expense = await AddEntity(CreateExpense());

            await UpdateEntity(expense);

            expense.ModifiedOnUtc.Should().NotBeNull();
            expense.ModifiedOnUtc.Should().NotBe(default);
            expense.ModifiedOnUtc.Should().BeAfter(expense.CreatedOnUtc);
        }

        [Fact]
        public async Task Should_set_created_on_date_when_adding_new_user()
        {
            var user = CreateUser();

            await AddEntity(user);

            user.CreatedOnUtc.Should().NotBe(default);
            user.ModifiedOnUtc.Should().BeNull();
        }

        [Fact]
        public async Task Should_set_modified_on_date_when_updating_existing_user()
        {
            var user = await AddEntity(CreateUser());

            await UpdateEntity(user);

            user.ModifiedOnUtc.Should().NotBeNull();
            user.ModifiedOnUtc.Should().NotBe(default);
            user.ModifiedOnUtc.Should().BeAfter(user.CreatedOnUtc);
        }

        private async Task<T> AddEntity<T>(T entity)
            where T : Entity
        {
            DbContext.Add(entity);

            await DbContext.SaveChangesAsync();

            return entity;
        }

        private async Task UpdateEntity(Entity entity)
        {
            DbContext.Update(entity);

            await DbContext.SaveChangesAsync();
        }

        private static Expense CreateExpense()
            => new Expense(Guid.NewGuid(), "Name", new Money(1.0m, Currency.FromId(1)!), DateTime.Now);

        private static User CreateUser()
            => new User(Guid.NewGuid(), "FirstName", "LastName", Email.Create("test@expensely.com").Value());
    }
}
