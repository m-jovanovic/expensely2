using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Expenses;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.Expenses.Commands.DeleteExpense;
using Expensely.Application.Expenses.Queries.GetExpenseById;
using Expensely.Application.Expenses.Queries.GetExpenses;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers
{
    public class ExpensesControllerTests
    {
        [Fact]
        public async Task Get_expenses_should_return_not_found_if_query_returns_empty_collection()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetExpensesQuery>(), default))
                .ReturnsAsync(new List<ExpenseResponse>());
            var controller = new ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.GetExpenses();

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_expenses_should_return_ok_if_query_returns_non_empty_collection()
        {
            var mediatorMock = new Mock<IMediator>();
            var expenseResponses = new List<ExpenseResponse> { new ExpenseResponse() };
            mediatorMock.Setup(x => x.Send(It.IsAny<GetExpensesQuery>(), default))
                .ReturnsAsync(expenseResponses);
            var controller = new ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.GetExpenses();

            OkObjectResult okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            IReadOnlyCollection<ExpenseResponse> value = okObjectResult.Value.As<IReadOnlyCollection<ExpenseResponse>>();
            value.Should().NotBeNull();
            value.Should().HaveCount(expenseResponses.Count);
        }

        [Fact]
        public async Task Get_expense_by_id_should_return_not_found_if_query_returns_null()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetExpenseByIdQuery>(), default))
                .ReturnsAsync((ExpenseResponse?)null);
            var controller = new ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.GetExpenseById(Guid.NewGuid());

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_expense_by_id_should_return_ok_if_query_returns_expense_response()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<GetExpenseByIdQuery>(), default))
                .ReturnsAsync(new ExpenseResponse());
            var controller = new ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.GetExpenseById(Guid.NewGuid());

            OkObjectResult okObjectResult = result.As<OkObjectResult>();
            okObjectResult.Should().NotBeNull();
            ExpenseResponse? value = okObjectResult.Value.As<ExpenseResponse?>();
            value.Should().NotBeNull();
        }

        [Fact]
        public async Task Get_expense_by_id_should_send_valid_query()
        {
            var mediatorMock = new Mock<IMediator>();
            var controller = new ExpensesController(mediatorMock.Object);
            var expenseId = Guid.NewGuid();

            await controller.GetExpenseById(expenseId);

            mediatorMock.Verify(x => x.Send(It.Is<GetExpenseByIdQuery>(q => q.ExpenseId == expenseId), default), Times.Once);
        }

        [Fact]
        public async Task Create_expense_should_return_bad_request_if_request_is_null()
        {
            var controller = new ExpensesController(new Mock<IMediator>().Object);

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
            var controller = new ExpensesController(mediatorMock.Object);

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
            var controller = new ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.CreateExpense(CreateRequest());

            CreatedAtActionResult createdAtActionResult = result.As<CreatedAtActionResult>();
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ControllerName.Should().BeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(ExpensesController.GetExpenseById));
            createdAtActionResult.RouteValues.Keys.Should().Contain("id");
            createdAtActionResult.RouteValues.Values.Should().Contain(entityCreatedResponse.Id);
        }

        [Fact]
        public async Task Create_expense_should_send_valid_command()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<CreateExpenseCommand>(), default))
                .ReturnsAsync(Result.Ok(new EntityCreatedResponse(Guid.NewGuid())));
            var controller = new ExpensesController(mediatorMock.Object);
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

        [Fact]
        public async Task Delete_expense_should_return_not_found_if_command_returns_failure_result()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteExpenseCommand>(), default))
                .ReturnsAsync(Result.Fail(Errors.General.EntityNotFound));
            var controller = new ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.DeleteExpense(Guid.NewGuid());

            result.Should().NotBeNull();
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_expense_should_return_no_content_if_command_returns_success_result()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteExpenseCommand>(), default))
                .ReturnsAsync(Result.Ok);
            var controller = new ExpensesController(mediatorMock.Object);

            IActionResult result = await controller.DeleteExpense(Guid.NewGuid());

            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_expense_should_send_valid_command()
        {
            var mediatorMock = new Mock<IMediator>();
            mediatorMock.Setup(x => x.Send(It.IsAny<DeleteExpenseCommand>(), default))
                .ReturnsAsync(Result.Ok);
            var controller = new ExpensesController(mediatorMock.Object);
            var expenseId = Guid.NewGuid();

            await controller.DeleteExpense(expenseId);

            mediatorMock.Verify(x => x.Send(It.Is<DeleteExpenseCommand>(c => c.ExpenseId == expenseId), default), Times.Once);
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
