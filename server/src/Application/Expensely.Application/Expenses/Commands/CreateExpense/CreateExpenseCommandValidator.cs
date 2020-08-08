using Expensely.Domain;
using FluentValidation;

namespace Expensely.Application.Expenses.Commands.CreateExpense
{
    /// <summary>
    /// Represents the validator for the <see cref="CreateExpenseCommand"/>.
    /// </summary>
    public sealed class CreateExpenseCommandValidator : AbstractValidator<CreateExpenseCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateExpenseCommandValidator"/> class.
        /// </summary>
        public CreateExpenseCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithErrorCode(Errors.Expense.UserIdIsRequired);

            RuleFor(x => x.CurrencyId).NotEmpty().WithErrorCode(Errors.Expense.CurrencyIsRequired);

            RuleFor(x => x.Date).NotEmpty().WithErrorCode(Errors.Expense.DateIsRequired);
        }
    }
}
