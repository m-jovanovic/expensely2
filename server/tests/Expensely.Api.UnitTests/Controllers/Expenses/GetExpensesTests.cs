using System;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Infrastructure.Services.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Expenses
{
    public class GetExpensesTests
    {
        private const int Limit = 1;
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly IDateTime _dateTime;

        public GetExpensesTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(UserId);
            _dateTime = new MachineDateTime();
        }

        [Fact]
        public async Task Should_return_not_found_if_user_id_does_not_equal_authenticated_user_id()
        {
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            var result = await controller.GetExpenses(Guid.NewGuid(), Limit, null);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Should_send_valid_query()
        {
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            await controller.GetExpenses(UserId, Limit, null);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetExpensesQuery>(q => q.UserId == UserId && q.Limit == Limit + 1), default),
                Times.Once);
        }

        [Fact]
        public async Task Should_return_ok_if_user_id_equals_authenticated_user_id()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpensesQuery>(), default))
                .ReturnsAsync(new ExpenseListResponse(Array.Empty<ExpenseResponse>()));
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            var result = await controller.GetExpenses(UserId, Limit, null);

            var okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            var expenseListResponse = okObjectResult.Value.As<ExpenseListResponse>();
            expenseListResponse.Should().NotBeNull();
            expenseListResponse.Items.Should().BeEmpty();
            expenseListResponse.Cursor.Should().BeEmpty();
        }
    }
}
