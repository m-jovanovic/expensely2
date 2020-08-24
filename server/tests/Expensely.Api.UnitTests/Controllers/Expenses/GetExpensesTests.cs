using System;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Infrastructure.Services.Common;
using MediatR;
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
        public async Task Get_expenses_should_send_valid_query()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpensesQuery>(), default))
                .ReturnsAsync(new ExpenseListResponse(Array.Empty<ExpenseResponse>()));
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            await controller.GetExpenses(UserId, Limit, null);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetExpensesQuery>(q => q.UserId == UserId && q.Limit == Limit + 1), default),
                Times.Once);
        }
    }
}
