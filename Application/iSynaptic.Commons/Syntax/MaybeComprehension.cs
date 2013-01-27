using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Syntax
{
    public static class MaybeComprehension
    {
        public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> @this, Func<T, Maybe<TResult>> selector)
        {
            return Maybe.SelectMaybe(@this, selector);
        }

        public static Maybe<TResult> SelectMany<T, TIntermediate, TResult>(this Maybe<T> @this, Func<T, Maybe<TIntermediate>> selector, Func<T, TIntermediate, TResult> combiner)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(combiner, "combiner");

            return Maybe.SelectMaybe(@this, x => selector(x).Select(y => combiner(x, y)));
        }

        public static IEnumerable<TResult> SelectMany<T, TIntermediate, TResult>(this Maybe<T> @this, Func<T, IEnumerable<TIntermediate>> selector, Func<T, TIntermediate, TResult> combinder)
        {
            Guard.NotNull(selector, "selector");
            Guard.NotNull(combinder, "combiner");

            return @this.Select(x => selector(x).Select(y => combinder(x, y))).Squash();
        }
    }
}
