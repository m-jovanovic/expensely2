using System;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Data;
using Expensely.Application.Transactions.Queries.GetCurrentWeekBalance;
using FluentAssertions;
using Moq;
using Xunit;

namespace Expensely.Application.UnitTests.Transactions.Queries
{
    public class GetCurrentWeeklyBalanceQueryTests
    {
        private readonly Mock<IDbContext> _dbContextMock;

        public GetCurrentWeeklyBalanceQueryTests() => _dbContextMock = new Mock<IDbContext>();

        [Fact]
        public async Task Should_return_null_if_user_id_is_empty()
        {
            var queryHandler = new GetCurrentWeekBalanceQueryHandler(_dbContextMock.Object);
            var query = new GetCurrentWeekBalanceQuery(Guid.Empty, default, DateTime.UtcNow);

            BalanceResponse? result = await queryHandler.Handle(query, default);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_return_null_if_currency_id_is_invalid()
        {
            var queryHandler = new GetCurrentWeekBalanceQueryHandler(_dbContextMock.Object);
            var query = new GetCurrentWeekBalanceQuery(Guid.NewGuid(), default, DateTime.UtcNow);

            BalanceResponse? result = await queryHandler.Handle(query, default);

            result.Should().BeNull();
        }
    }
}
