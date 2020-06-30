using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Extensions;
using Expensely.Application.Interfaces;
using MediatR;

namespace Expensely.Application.Behaviours
{
    internal sealed class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnitOfWorkBehaviour(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response = await next();

            if (request.IsQuery())
            {
                return response;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
