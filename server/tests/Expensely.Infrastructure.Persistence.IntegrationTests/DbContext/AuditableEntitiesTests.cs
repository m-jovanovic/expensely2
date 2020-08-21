using System.Threading.Tasks;
using Expensely.Infrastructure.Persistence.IntegrationTests.Common;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.DbContext
{
    public class AuditableEntitiesTests : DbContextTest
    {
        [Fact]
        public async Task Should_set_created_on_date_when_adding_new_expense()
        {
            var expense = await InsertAsync(TransactionData.CreateExpense());
            
            expense.CreatedOnUtc.Should().NotBe(default);
            expense.ModifiedOnUtc.Should().BeNull();
        }

        [Fact]
        public async Task Should_set_modified_on_date_when_updating_existing_expense()
        {
            var expense = await InsertAsync(TransactionData.CreateExpense());

            await UpdateAsync(expense);

            expense.ModifiedOnUtc.Should().NotBeNull();
            expense.ModifiedOnUtc.Should().NotBe(default);
            expense.ModifiedOnUtc.Should().BeAfter(expense.CreatedOnUtc);
        }

        [Fact]
        public async Task Should_set_created_on_date_when_adding_new_user()
        {
            var user = await InsertAsync(UserData.CreateUser());

            user.CreatedOnUtc.Should().NotBe(default);
            user.ModifiedOnUtc.Should().BeNull();
        }

        [Fact]
        public async Task Should_set_modified_on_date_when_updating_existing_user()
        {
            var user = await InsertAsync(UserData.CreateUser());

            await UpdateAsync(user);

            user.ModifiedOnUtc.Should().NotBeNull();
            user.ModifiedOnUtc.Should().NotBe(default);
            user.ModifiedOnUtc.Should().BeAfter(user.CreatedOnUtc);
        }
    }
}
