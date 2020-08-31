using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Application.Incomes.Commands.CreateIncome;
using Expensely.Application.Incomes.Events.IncomeCreated;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Transactions;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using static Expensely.Tests.Common.Commands.Expenses.CreateIncomeCommandData;

namespace Expensely.Application.UnitTests.Incomes.Commands
{
    public class CreateIncomeCommandTests
    {
        [Fact]
        public async Task Handle_should_fail_if_currency_id_is_invalid()
        {
            var incomeRepositoryMock = new Mock<IIncomeRepository>();
            var commandHandler = new CreateIncomeCommandHandler(
                incomeRepositoryMock.Object,
                new Mock<IMediator>().Object);
            var command = CreateCommandWithInvalidCurrencyId();

            Result<EntityCreatedResponse> result = await commandHandler.Handle(command, default);

            result.Error.Should().Be(Errors.Currency.NotFound);
        }

        [Fact]
        public async Task Handle_should_call_insert_on_income_repository()
        {
            var incomeRepositoryMock = new Mock<IIncomeRepository>();
            var commandHandler = new CreateIncomeCommandHandler(
                incomeRepositoryMock.Object,
                new Mock<IMediator>().Object);
            var command = CreateValidCommand();

            Result<EntityCreatedResponse> result = await commandHandler.Handle(command, default);

            incomeRepositoryMock.Verify(x => x.Insert(It.Is<Income>(e => e.Id == result.Value().Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_publish_income_created_event()
        {
            var mediatorMock = new Mock<IMediator>();
            var commandHandler = new CreateIncomeCommandHandler(
                new Mock<IIncomeRepository>().Object,
                mediatorMock.Object);
            var command = CreateValidCommand();

            Result<EntityCreatedResponse> result = await commandHandler.Handle(command, default);

            mediatorMock.Verify(
                x => x.Publish(It.Is<IncomeCreatedEvent>(e => e.IncomeId == result.Value().Id), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_should_complete_successfully_if_command_is_valid()
        {
            var incomeRepositoryMock = new Mock<IIncomeRepository>();
            var commandHandler = new CreateIncomeCommandHandler(
                incomeRepositoryMock.Object,
                new Mock<IMediator>().Object);
            var command = CreateValidCommand();

            Result<EntityCreatedResponse> result = await commandHandler.Handle(command, default);

            result.IsSuccess.Should().BeTrue();
            EntityCreatedResponse entityCreatedResponse = result.Value();
            entityCreatedResponse.Should().NotBeNull();
            entityCreatedResponse.Id.Should().NotBe(Guid.Empty);
        }
    }
}
