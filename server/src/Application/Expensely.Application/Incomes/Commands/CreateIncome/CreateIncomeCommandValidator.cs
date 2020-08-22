using Expensely.Application.Core.Utilities;
using Expensely.Application.Expenses.Commands.CreateExpense;
using Expensely.Domain;
using FluentValidation;

namespace Expensely.Application.Incomes.Commands.CreateIncome
{
    /// <summary>
    /// Represents the validator for the <see cref="CreateExpenseCommand"/>.
    /// </summary>
    public sealed class CreateIncomeCommandValidator : AbstractValidator<CreateIncomeCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateIncomeCommandValidator"/> class.
        /// </summary>
        public CreateIncomeCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithError(Errors.Income.UserIdIsRequired);

            RuleFor(x => x.CurrencyId).NotEmpty().WithError(Errors.Income.CurrencyIdIsRequired);

            RuleFor(x => x.OccurredOn).NotEmpty().WithError(Errors.Income.OccurredOnIsRequired);
        }
    }
}
