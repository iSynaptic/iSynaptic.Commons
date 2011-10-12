using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Syntax
{
    public static class OutcomeComprehension
    {
        public static Outcome<T> Return<T>(T value)
        {
            return Outcome.Success(value);
        }

        public static Outcome<U> Bind<T, U>(this Outcome<T> @this, Func<T, Outcome<U>> selector)
        {
            return Outcome.InformMany(@this, selector);
        }

        public static Outcome<TResult> Extend<T, TResult>(this Outcome<T> @this, Func<Outcome<T>, TResult> selector)
        {
            Guard.NotNull(selector, "selector");
            var self = @this;

            return new Outcome<TResult>(() => Return(selector(self)));
        }

        public static T Extract<T>(this Outcome<T> @this)
        {
            return @this.Observations.First();
        }

        public static Outcome<T> Where<T>(this Outcome<T> @this, Func<T, bool> predicate)
        {
            return Outcome.Notice(@this, predicate);
        }

        public static Outcome<T> Unless<T>(this Outcome<T> @this, Func<T, bool> predicate)
        {
            return Outcome.Ignore(@this, predicate);
        }

        public static Outcome<U> SelectMany<T, U>(this Outcome<T> @this, Func<T, Outcome<U>> selector)
        {
            return Outcome.InformMany(@this, selector);
        }

        public static Outcome<U> Select<T, U>(this Outcome<T> @this, Func<T, U> selector)
        {
            return Outcome.InformMany(@this, t => new Outcome<U>(@this.WasSuccessful, selector(t)));
        }
    }
}
