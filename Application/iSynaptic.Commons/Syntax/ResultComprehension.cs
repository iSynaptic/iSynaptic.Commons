using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Syntax
{
    public static class ResultComprehension
    {
        public static Result<TResult, TObservation> SelectMany<T, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TResult, TObservation>> selector)
        {
            return Result.SelectResult(@this, selector);
        }

        public static Result<TResult, TObservation> SelectMany<T, TIntermediate, TResult, TObservation>(this Result<T, TObservation> @this, Func<T, Result<TIntermediate, TObservation>> selector, Func<T, TIntermediate, TResult> combiner)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(combiner, "combiner");

            return Result.SelectResult(@this, x => selector(x).Select(y => combiner(x, y)));
        }
    }
}
