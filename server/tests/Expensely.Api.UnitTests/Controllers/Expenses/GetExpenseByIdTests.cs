using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Transactions;
using Expensely.Infrastructure.Services.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Expenses
{
    public class GetExpenseByIdTests
    {
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly IDateTime _dateTime;

        public GetExpenseByIdTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(UserId);
            _dateTime = new MachineDateTime();
        }

        [Fact]
        public async Task Should_send_valid_query()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpenseByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<ExpenseResponse>(Errors.General.EntityNotFound));
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);
            var expenseId = Guid.NewGuid();

            await controller.GetExpenseById(expenseId);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetExpenseByIdQuery>(q => q.ExpenseId == expenseId && q.UserId == UserId), default),
                Times.Once);
        }

        [Fact]
        public async Task Should_return_not_found_if_query_returns_failure_result()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpenseByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Failure<ExpenseResponse>(Errors.General.EntityNotFound));
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            var result = await controller.GetExpenseById(Guid.NewGuid());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Should_return_ok_if_query_returns_success_result()
        {
            var response = new ExpenseResponse(Guid.NewGuid(), "Expense", -1.0m, Currency.Usd.Value, DateTime.Now, DateTime.Now);
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpenseByIdQuery>(), default))
                .ReturnsAsync(response);
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            var result = await controller.GetExpenseById(Guid.NewGuid());

            OkObjectResult okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            ExpenseResponse? value = okObjectResult.Value.As<ExpenseResponse?>();
            value.Should().NotBeNull();
        }
    }
}
