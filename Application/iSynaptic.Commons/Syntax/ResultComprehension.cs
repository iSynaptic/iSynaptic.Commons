using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Syntax
{
    public static class ResultComprehension
    {
        public static Result<T, Unit> Return<T>(T value)
        {
            return Result.Value(value);
        }

        public static Result<T, TObservation> Return<T, TObservation>(T value)
        {
            return Result.Value<T, TObservation>(value);
        }

        public static Result<TResult, TObservation> Bind<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TResult, TObservation>> selector)
        {
            return Result.SelectResult(@this, selector);
        }

        public static Result<TResult, Unit> Bind<T, TResult>(this Result<T, Unit> @this, Func<T, Result<TResult, Unit>> selector)
        {
            return Result.SelectResult(@this, selector);
        }

        #region SelectMany Operator

        public static Result<TResult, TObservation> SelectMany<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TResult, TObservation>> selector)
        {
            return Bind(@this, selector);
        }

        public static Result<TResult, TObservation> SelectMany<T, TIntermediate, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TIntermediate, TObservation>> selector, Func<T, TIntermediate, TResult> combiner)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(combiner, "combiner");

            return @this.SelectMany(x => selector(x).Select(y => combiner(x, y)));
        }

        #endregion
    }
}
