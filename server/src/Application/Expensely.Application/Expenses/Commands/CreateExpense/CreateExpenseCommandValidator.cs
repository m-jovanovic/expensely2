using Expensely.Application.Core.Utilities;
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
            RuleFor(x => x.UserId).NotEmpty().WithError(Errors.Expense.UserIdIsRequired);

            RuleFor(x => x.CurrencyCode).NotEmpty().WithError(Errors.Expense.CurrencyIsRequired);

            RuleFor(x => x.OccurredOn).NotEmpty().WithError(Errors.Expense.OccurredOnIsRequired);
        }
    }
}
