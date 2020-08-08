﻿using System;
using System.Threading.Tasks;
using Expensely.Api.Controllers;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Queries.GetExpenseById;
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

        public GetExpenseByIdTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(UserId);
        }

        [Fact]
        public async Task Get_expense_by_id_should_return_not_found_if_query_returns_null()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpenseByIdQuery>(), default))
                .ReturnsAsync((ExpenseResponse?)null);
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);

            IActionResult result = await controller.GetExpenseById(Guid.NewGuid());

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_expense_by_id_should_return_ok_if_query_returns_expense_response()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<GetExpenseByIdQuery>(), default))
                .ReturnsAsync(new ExpenseResponse());
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);

            IActionResult result = await controller.GetExpenseById(Guid.NewGuid());

            OkObjectResult okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            ExpenseResponse? value = okObjectResult.Value.As<ExpenseResponse?>();
            value.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_expense_by_id_should_send_valid_query()
        {
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);
            var expenseId = Guid.NewGuid();

            await controller.GetExpenseById(expenseId);

            _mediatorMock.Verify(
                x => x.Send(It.Is<GetExpenseByIdQuery>(q => q.ExpenseId == expenseId && q.UserId == UserId), default),
                Times.Once);
        }
    }
}
