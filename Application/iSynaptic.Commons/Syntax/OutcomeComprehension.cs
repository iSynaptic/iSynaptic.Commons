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


        public static Outcome<U> SelectMany<T, U>(this Outcome<T> @this, Func<T, Outcome<U>> selector)
        {
            return Outcome.InformMany(@this, selector);
        }

        public static Outcome<U> Select<T, U>(this Outcome<T> @this, Func<T, U> selector)
        {
            return Outcome.InformMany(@this, t => new Outcome<U>(@this.WasSuccessful, new[] { selector(t) }));
        }
    }
}
