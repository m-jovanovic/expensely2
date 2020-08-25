using System;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Contracts.Incomes;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Incomes.Queries.GetExpenseById;
using Expensely.Domain.Transactions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Incomes
{
    public class GetIncomeByIdTests
    {
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;

        public GetIncomeByIdTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(UserId);
        }

        [Fact]
        public async Task Should_send_valid_query()
        {
            var controller = new IncomesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);
            var incomeId = Guid.NewGuid();

            await controller.GetIncomeById(incomeId);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetIncomeByIdQuery>(q => q.IncomeId == incomeId && q.UserId == UserId), default),
                Times.Once);
        }

        [Fact]
        public async Task Should_return_not_found_if_query_returns_null()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetIncomeByIdQuery>(), default))
                .ReturnsAsync((IncomeResponse?)null);
            var controller = new IncomesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);

            var result = await controller.GetIncomeById(Guid.NewGuid());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Should_return_ok_if_query_returns_income_response()
        {
            var response = new IncomeResponse(Guid.NewGuid(), "Income", 1.0m, Currency.Usd.Value, DateTime.Now, DateTime.Now);
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetIncomeByIdQuery>(), default))
                .ReturnsAsync(response);
            var controller = new IncomesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);

            var result = await controller.GetIncomeById(UserId);

            OkObjectResult okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            IncomeResponse? value = okObjectResult.Value.As<IncomeResponse?>();
            value.Should().NotBeNull();
        }
    }
}
