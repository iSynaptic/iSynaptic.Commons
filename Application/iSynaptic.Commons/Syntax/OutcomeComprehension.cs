using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Syntax
{
    public static class OutcomeComprehension
    {
        public static Outcome<T> Where<T>(this Outcome<T> @this, Func<T, bool> predicate)
        {
            return Outcome.Notice(@this, predicate);
        }

        public static Outcome<TResult> SelectMany<TObservation, TIntermediate, TResult>(this Outcome<TObservation> @this, Func<TObservation, Outcome<TIntermediate>> selector, Func<TObservation, TIntermediate, TResult> combiner)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(combiner, "combiner");

            return Outcome.InformMany(@this, x => selector(x).Select(y => combiner(x, y)));
        }

        public static Outcome<TResult> SelectMany<T, TResult>(this Outcome<T> @this, Func<T, Outcome<TResult>> selector)
        {
            return Outcome.InformMany(@this, selector);
        }

        public static Outcome<TResult> Select<T, TResult>(this Outcome<T> @this, Func<T, TResult> selector)
        {
            return Outcome.InformMany(@this, t => new Outcome<TResult>(@this.WasSuccessful, new[] { selector(t) }));
        }
    }
}
