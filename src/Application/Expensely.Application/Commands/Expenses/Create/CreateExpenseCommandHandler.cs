using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Messaging;
using Expensely.Common.Primitives;

namespace Expensely.Application.Commands.Expenses.Create
{
    public sealed class CreateExpenseCommandHandler : ICommandHandler<CreateExpenseCommand, Result>
    {
        public async Task<Result> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);

            return Result.Ok();
        }
    }
}