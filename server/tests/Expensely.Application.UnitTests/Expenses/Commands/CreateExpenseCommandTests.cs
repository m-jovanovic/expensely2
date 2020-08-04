using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Repositories;
using Expensely.Application.Contracts.Common;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Application.Expenses.Events.ExpenseCreated;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using static Expensely.Tests.Common.Commands.Expenses.ExpenseCommandsData;

namespace Expensely.Application.UnitTests.Expenses.Commands
{
    public class CreateExpenseCommandTests
    {
        [Fact]
        public async Task Handle_should_fail_if_currency_id_is_invalid()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryMock.Object, new Mock<IMediator>().Object);
            var command = CreateExpenseCommandWithInvalidCurrencyId();

            Result<EntityCreatedResponse> result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.Currency.NotFound);
            result.Invoking(r => r.Value()).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task Handle_should_call_insert_on_expense_repository()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryMock.Object, new Mock<IMediator>().Object);
            var command = ValidCreateExpenseCommand();

            Result<EntityCreatedResponse> result = await commandHandler.Handle(command, default);

            expenseRepositoryMock.Verify(x => x.Insert(It.Is<Expense>(e => e.Id == result.Value().Id)), Times.Once);
        }

        [Fact]
        public async Task Handle_should_publish_expense_created_event()
        {
            var mediatorMock = new Mock<IMediator>();
            var commandHandler = new CreateExpenseCommandHandler(new Mock<IExpenseRepository>().Object, mediatorMock.Object);
            var command = ValidCreateExpenseCommand();

            Result<EntityCreatedResponse> result = await commandHandler.Handle(command, default);

            mediatorMock.Verify(
                x => x.Publish(It.Is<ExpenseCreatedEvent>(e => e.ExpenseId == result.Value().Id), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_should_complete_successfully_if_command_is_valid()
        {
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var commandHandler = new CreateExpenseCommandHandler(expenseRepositoryMock.Object, new Mock<IMediator>().Object);
            var command = ValidCreateExpenseCommand();

            Result<EntityCreatedResponse> result = await commandHandler.Handle(command, default);

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            result.Invoking(r => r.Value()).Should().NotThrow();
            EntityCreatedResponse entityCreatedResponse = result.Value();
            entityCreatedResponse.Should().NotBeNull();
            entityCreatedResponse.Id.Should().NotBe(Guid.Empty);
        }
    }
}
