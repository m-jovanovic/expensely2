using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Core.Extensions;
using Expensely.Application.Transactions.Queries.GetCurrentWeekBalance;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Transactions;
using Expensely.Infrastructure.Services.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Transactions
{
    public class GetCurrentWeekBalanceQueryTests
    {
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly IDateTime _dateTime;

        public GetCurrentWeekBalanceQueryTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(UserId);
            _dateTime = new MachineDateTime();
        }

        [Fact]
        public async Task Should_return_not_found_if_user_id_does_not_equal_authenticated_user_id()
        {
            var controller = new TransactionsController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            var result = await controller.GetCurrentWeekBalance(Guid.NewGuid(), default);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Should_send_valid_query()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetCurrentWeekBalanceQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<BalanceResponse>(Errors.General.EntityNotFound));
            int currencyId = Currency.Usd.Value;
            var controller = new TransactionsController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);
            
            await controller.GetCurrentWeekBalance(UserId, currencyId);

            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<GetCurrentWeekBalanceQuery>(q =>
                        q.UserId == UserId &&
                        q.CurrencyId == currencyId &&
                        q.FirstDayOfWeek == _dateTime.UtcNow.StartOfWeek()),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task Should_return_not_found_if_query_returns_null()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetCurrentWeekBalanceQuery>(), default))
                .ReturnsAsync(Result.Failure<BalanceResponse>(Errors.General.EntityNotFound));
            var controller = new TransactionsController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            var result = await controller.GetCurrentWeekBalance(UserId, default);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Should_return_ok_if_query_returns_balance_response()
        {
            Currency currency = Currency.Usd;
            const decimal amount = 100.0m;
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetCurrentWeekBalanceQuery>(), default))
                .ReturnsAsync(new BalanceResponse(currency.Value, amount));
            var controller = new TransactionsController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            var result = await controller.GetCurrentWeekBalance(UserId, currency.Value);

            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            var balanceResponse = okObjectResult.Value.As<BalanceResponse>();
            balanceResponse.Should().NotBeNull();
            balanceResponse.Amount.Should().Be(amount);
            balanceResponse.Balance.Should().Be(currency.Format(amount));
        }
    }
}
