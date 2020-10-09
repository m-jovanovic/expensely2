using System;
using System.Threading.Tasks;
using Expensely.Api.Contracts;
using Expensely.Api.Controllers;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Contracts.Incomes;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.Incomes.Commands.CreateIncome;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Transactions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Expensely.Api.UnitTests.Controllers.Incomes
{
    public class CreateIncomeTests
    {
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUserIdentifierProvider> _userIdentifierProviderMock;

        public CreateIncomeTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _userIdentifierProviderMock = new Mock<IUserIdentifierProvider>();
            _userIdentifierProviderMock.SetupGet(x => x.UserId).Returns(UserId);
        }

        [Fact]
        public async Task Should_return_bad_request_if_request_is_null()
        {
            var controller = new IncomesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);

            IActionResult result = await controller.CreateIncome(null);

            BadRequestObjectResult badRequestObjectResult = result.As<BadRequestObjectResult>();
            badRequestObjectResult.Should().NotBeNull();
            ApiErrorResponse apiErrorResponse = badRequestObjectResult.Value.As<ApiErrorResponse>();
            apiErrorResponse.Should().NotBeNull();
            apiErrorResponse.Errors.Should().Contain(Errors.General.BadRequest);
        }

        [Fact]
        public async Task Should_send_valid_command()
        {
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateIncomeCommand>(), default))
                .ReturnsAsync(Result.Success(new EntityCreatedResponse(Guid.NewGuid())));
            var controller = new IncomesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);
            CreateIncomeRequest request = CreateRequest();

            await controller.CreateIncome(request);

            _mediatorMock.Verify(
                x => x.Send(
                    It.Is<CreateIncomeCommand>(c =>
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
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateIncomeCommand>(), default)).ReturnsAsync(failureResult);
            var controller = new IncomesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);

            IActionResult result = await controller.CreateIncome(CreateRequest());

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
            _mediatorMock.Setup(x => x.Send(It.IsAny<CreateIncomeCommand>(), default))
                .ReturnsAsync(Result.Success(entityCreatedResponse));
            var controller = new IncomesController(_mediatorMock.Object, _userIdentifierProviderMock.Object);

            IActionResult result = await controller.CreateIncome(CreateRequest());

            CreatedAtActionResult createdAtActionResult = result.As<CreatedAtActionResult>();
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult.ControllerName.Should().BeNull();
            createdAtActionResult.ActionName.Should().Be(nameof(IncomesController.GetIncomeById));
            createdAtActionResult.RouteValues.Keys.Should().Contain("id");
            createdAtActionResult.RouteValues.Values.Should().Contain(entityCreatedResponse.Id);
        }

        private static CreateIncomeRequest CreateRequest()
            => new CreateIncomeRequest
            {
                Name = "Income",
                Amount = 1.0m,
                CurrencyId = Currency.Usd.Value,
                Date = DateTime.Now
            };
    }
}
