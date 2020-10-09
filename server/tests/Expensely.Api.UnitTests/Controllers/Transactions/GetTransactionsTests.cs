using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Transactions;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Transactions.Queries.GetTransactions;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Infrastructure.Services.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Transactions
{
    public class GetTransactionsTests
    {
        private const int Limit = 1;
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly IDateTime _dateTime;

        public GetTransactionsTests()
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

            var result = await controller.GetTransactions(Guid.NewGuid(), Limit, null);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Should_send_valid_query()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetTransactionsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<TransactionListResponse>(Errors.General.EntityNotFound));
            var controller = new TransactionsController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            await controller.GetTransactions(UserId, Limit, null);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetTransactionsQuery>(q => q.UserId == UserId && q.Limit == Limit + 1), default),
                Times.Once);
        }

        [Fact]
        public async Task Should_return_ok_if_user_id_equals_authenticated_user_id()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetTransactionsQuery>(), default))
                .ReturnsAsync(new TransactionListResponse(Array.Empty<TransactionResponse>()));
            var controller = new TransactionsController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            var result = await controller.GetTransactions(UserId, Limit, null);

            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            var expenseListResponse = okObjectResult.Value.As<TransactionListResponse>();
            expenseListResponse.Should().NotBeNull();
            expenseListResponse.Items.Should().BeEmpty();
            expenseListResponse.Cursor.Should().BeEmpty();
        }
    }
}
