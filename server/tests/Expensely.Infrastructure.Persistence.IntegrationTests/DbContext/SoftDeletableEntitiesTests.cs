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
    public class SoftDeletableEntitiesTests : DbContextTest
    {
        [Fact]
        public async Task Should_set_deleted_flag_and_date_when_deleting_expense()
        {
            var expense = await AddEntity(CreateExpense());

            await RemoveEntity(expense);

            expense.Deleted.Should().BeTrue();
            expense.DeletedOnUtc.Should().NotBeNull();
            expense.DeletedOnUtc.Should().NotBe(default);
        }

        [Fact]
        public async Task Should_set_deleted_flag_and_date_when_deleting_user()
        {
            var user = await AddEntity(CreateUser());

            await RemoveEntity(user);

            user.Deleted.Should().BeTrue();
            user.DeletedOnUtc.Should().NotBeNull();
            user.DeletedOnUtc.Should().NotBe(default);
        }

        private async Task<T> AddEntity<T>(T entity)
            where T : Entity
        {
            DbContext.Add(entity);

            await DbContext.SaveChangesAsync();

            return entity;
        }

        private async Task RemoveEntity(Entity entity)
        {
            DbContext.Remove(entity);

            await DbContext.SaveChangesAsync();
        }

        private static Expense CreateExpense()
            => new Expense(Guid.NewGuid(), "Name", new Money(1.0m, Currency.FromId(1)!), DateTime.Now);

        private static User CreateUser()
            => new User(Guid.NewGuid(), "FirstName", "LastName", Email.Create("test@expensely.com").Value());
    }
}
