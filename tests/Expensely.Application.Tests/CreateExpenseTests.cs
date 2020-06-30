using Expensely.Application.Commands.Expenses.CreateExpense;
using Expensely.Application.Interfaces;
using Expensely.Common.Primitives;
using Expensely.Domain.Entities;
using Moq;
using Xunit;

namespace Expensely.Application.Tests
{
    public sealed class CreateExpenseTests
    {
        [Fact]
        public void Should_CreateCommandProperly()
        {
            var command = new CreateExpenseCommand(100.0m);

            Assert.Equal(100.0m, command.Amount);
        }

        [Fact]
        public async void Should_HandleCommandSuccessfully()
        {
            var expenseRepository = new Mock<IExpenseRepository>();
            var command = new CreateExpenseCommand(100.0m);
            var commandHandler = new CreateExpenseCommandHandler(expenseRepository.Object);

            Result result = await commandHandler.Handle(command, default).ConfigureAwait(false);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Should_CallInsertOnExpenseRepository()
        {
            var expenseRepository = new Mock<IExpenseRepository>();
            var command = new CreateExpenseCommand(100.0m);
            var commandHandler = new CreateExpenseCommandHandler(expenseRepository.Object);

            await commandHandler.Handle(command, default).ConfigureAwait(false);

            expenseRepository.Verify(x => x.Insert(It.IsAny<Expense>()), Times.Once());
        }
    }
}
