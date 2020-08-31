using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Common;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Transactions;
using Expensely.Infrastructure.Services.Common;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Expenses
{
    public class CreateExpenseTests
    {
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;
        private readonly IDateTime _dateTime;

        public CreateExpenseTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(UserId);
            _dateTime = new MachineDateTime();
        }

        [Fact]
        public async Task Should_return_bad_request_if_request_is_null()
        {
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            IActionResult result = await controller.CreateExpense(null);

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Errors.Should().Contain(Errors.General.BadRequest);
        }

        [Fact]
        public async Task Should_send_valid_command()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateExpenseCommand>(), default))
                .ReturnsAsync(Result.Success(new EntityCreatedResponse(Guid.NewGuid())));
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);
            CreateExpenseRequest request = CreateRequest();

            await controller.CreateExpense(request);

            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<CreateExpenseCommand>(c =>
                        c.UserId == UserId &&
                        c.Name == request.Name &&
                        c.Amount == request.Amount &&
                        c.CurrencyId == request.CurrencyId &&
                        c.OccurredOn == request.Date),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task Should_return_bad_request_if_command_returns_failure_result()
        {
            var failureResult = Result.Failure<EntityCreatedResponse>(Errors.Currency.NotFound);
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateExpenseCommand>(), default)).ReturnsAsync(failureResult);
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            IActionResult result = await controller.CreateExpense(CreateRequest());

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Errors.Should().Contain(failureResult.Error);
        }

        [Fact]
        public async Task Should_return_created_at_action_if_command_returns_success_result()
        {
            var entityCreatedResponse = new EntityCreatedResponse(Guid.NewGuid());
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateExpenseCommand>(), default))
                .ReturnsAsync(Result.Success(entityCreatedResponse));
            var controller = new ExpensesController(_mediatorMock.Object, _userIdentifierProviderMock.Object, _dateTime);

            IActionResult result = await controller.CreateExpense(CreateRequest());

            CreatedAtActionResult createdAtActionResult = result.As<CreatedAtActionResult>();
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ControllerName.Should().BeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(ExpensesController.GetExpenseById));
            createdAtActionResult.RouteValues.Keys.Should().Contain("id");
            createdAtActionResult.RouteValues.Values.Should().Contain(entityCreatedResponse.Id);
        }

        private static CreateExpenseRequest CreateRequest()
            => new CreateExpenseRequest
            {
                Name = "Expense",
                Amount = -1.0m,
                CurrencyId = Currency.Usd.Value,
                Date = DateTime.Now
            };
    }
}
