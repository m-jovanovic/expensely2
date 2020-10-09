using System;
using System.Threading.Tasks;

namespace Expensely.Domain.Core.Maybe.Extensions
{
    public static class MaybeExtensions
    {
        public static Maybe<T> Ensure<T>(this Maybe<T> maybe, Func<T, bool> predicate) =>
            maybe.HasValue && predicate(maybe.Value) ? maybe : Maybe<T>.None;

        public static Task<Maybe<TOut>> Bind<TIn, TOut>(this Maybe<TIn> maybe, Func<TIn, Task<Maybe<TOut>>> func) =>
            maybe.HasValue ? func(maybe.Value) : Task.FromResult(Maybe<TOut>.None);

        public static async Task<Maybe<TOut>> Bind<TIn, TOut>(this Task<Maybe<TIn>> maybeTask, Func<TIn, Maybe<TOut>> func)
        {
            Maybe<TIn> maybe = await maybeTask;

            return maybe.HasValue ? func(maybe.Value) : Maybe<TOut>.None;
        }

        public static async Task<TOut> Match<TIn, TOut>(this Task<Maybe<TIn>> maybeTask, Func<TIn, TOut> onHasValue, Func<TOut> onHasNoValue)
        {
            Maybe<TIn> maybe = await maybeTask;

            return maybe.HasValue ? onHasValue(maybe.Value) : onHasNoValue();
        }
    }
}
