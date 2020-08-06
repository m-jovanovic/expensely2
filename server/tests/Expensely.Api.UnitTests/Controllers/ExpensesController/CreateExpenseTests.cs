using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.ExpensesController
{
    public class CreateExpenseTests
    {
        [Fact]
        public async Task Create_expense_should_return_bad_request_if_request_is_null()
        {
            var controller = new Api.Controllers.ExpensesController(new Mock<IMediator>().Object);

            IActionResult result = await controller.CreateExpense(null);

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Success.Should().BeFalse();
            apiErrorResponse.Errors.Should().Contain(Errors.General.BadRequest);
        }

        [Fact]
        public async Task Create_expense_should_return_bad_request_if_command_returns_failure_result()
        {
            var mediatorMock = new Mock<IMediator>();
            var failureResult = Result.Fail<EntityCreatedResponse>(Errors.Currency.NotFound);
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateExpenseCommand>(), default)).ReturnsAsync(failureResult);
            var controller = new Api.Controllers.ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.CreateExpense(CreateRequest());

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Success.Should().BeFalse();
            apiErrorResponse.Errors.Should().Contain(failureResult.Error);
        }

        [Fact]
        public async Task Create_expense_should_return_created_at_action_if_command_returns_success_result()
        {
            var mediatorMock = new Mock<IMediator>();
            var entityCreatedResponse = new EntityCreatedResponse(Guid.NewGuid());
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateExpenseCommand>(), default))
                .ReturnsAsync(Result.Ok(entityCreatedResponse));
            var controller = new Api.Controllers.ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.CreateExpense(CreateRequest());

            CreatedAtActionResult createdAtActionResult = result.As<CreatedAtActionResult>();
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ControllerName.Should().BeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(Api.Controllers.ExpensesController.GetExpenseById));
            createdAtActionResult.RouteValues.Keys.Should().Contain("id");
            createdAtActionResult.RouteValues.Values.Should().Contain(entityCreatedResponse.Id);
        }

        [Fact]
        public async Task Create_expense_should_send_valid_command()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateExpenseCommand>(), default))
                .ReturnsAsync(Result.Ok(new EntityCreatedResponse(Guid.NewGuid())));
            var controller = new Api.Controllers.ExpensesController(mediatorMock.Object);
            CreateExpenseRequest createExpenseRequest = CreateRequest();

            await controller.CreateExpense(createExpenseRequest);

            mediatorMock.Verify(
                x => x.Send(
                    It.Is<CreateExpenseCommand>(c =>
                        c.Name == createExpenseRequest.Name &&
                        c.Amount == createExpenseRequest.Amount &&
                        c.CurrencyId == createExpenseRequest.CurrencyId &&
                        c.Date == createExpenseRequest.Date),
                    default),
                Times.Once);
        }

        private static CreateExpenseRequest CreateRequest()
            => new CreateExpenseRequest
            {
                Name = "Expense",
                Amount = 1.0m,
                CurrencyId = 1,
                Date = DateTime.Now
            };
    }
}
