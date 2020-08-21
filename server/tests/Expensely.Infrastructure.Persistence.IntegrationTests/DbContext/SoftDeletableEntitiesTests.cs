using System.Threading.Tasks;
using Expensely.Infrastructure.Persistence.IntegrationTests.Common;
using Expensely.Tests.Common.Entities;
using FluentAssertions;
using Xunit;

namespace Expensely.Infrastructure.Persistence.IntegrationTests.DbContext
{
    public class SoftDeletableEntitiesTests : DbContextTest
    {
        [Fact]
        public async Task Should_set_deleted_flag_and_date_when_deleting_expense()
        {
            var expense = await InsertAsync(TransactionData.CreateExpense());

            await RemoveAsync(expense);

            expense.Deleted.Should().BeTrue();
            expense.DeletedOnUtc.Should().NotBeNull();
            expense.DeletedOnUtc.Should().NotBe(default);
        }

        [Fact]
        public async Task Should_set_deleted_flag_and_date_when_deleting_user()
        {
            var user = await InsertAsync(UserData.CreateUser());

            await RemoveAsync(user);

            user.Deleted.Should().BeTrue();
            user.DeletedOnUtc.Should().NotBeNull();
            user.DeletedOnUtc.Should().NotBe(default);
        }
    }
}
