using Expensely.Application.Messaging;
using MediatR;

namespace Expensely.Application.Extensions
{
    public static class RequestExtensions
    {
        public static bool IsCommand<TResponse>(this IRequest<TResponse> request) => request is ICommand<TResponse>;

        public static bool IsQuery<TResponse>(this IRequest<TResponse> request) => request is IQuery<TResponse>;
    }
}
