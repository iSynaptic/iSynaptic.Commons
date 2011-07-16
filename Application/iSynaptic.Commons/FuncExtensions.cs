using System;
using System.Collections.Generic;

namespace iSynaptic.Commons
{
    public static partial class FuncExtensions
    {
        public static Action ToAction<TRet>(this Func<TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return () => @this();
        }

        public static Func<TRet> Curry<T1, TRet>(this Func<T1, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return () => @this(t1);
        }

        public static IComparer<T> ToComparer<T>(this Func<T, T, int> @this)
        {
            Guard.NotNull(@this, "@this");
            return new FuncComparer<T>(@this);
        }

        public static IEqualityComparer<T> ToEqualityComparer<T, TResult>(this Func<T, TResult> selector)
        {
            Guard.NotNull(selector, "selector");

            return ToEqualityComparer<T>((x, y) => EqualityComparer<TResult>.Default.Equals(selector(x), selector(y)),
                                          x => EqualityComparer<TResult>.Default.GetHashCode((selector(x))));
        }

        public static IEqualityComparer<T> ToEqualityComparer<T>(this Func<T, T, bool> equalsStrategy, Func<T, int> hashCodeStrategy)
        {
            Guard.NotNull(equalsStrategy, "equalsStrategy");
            Guard.NotNull(hashCodeStrategy, "hashCodeStrategy");

            return new FuncEqualityComparer<T>(equalsStrategy, hashCodeStrategy);
        }

        public static Func<TResult> Memoize<TResult>(this Func<TResult> @this)
        {
            Guard.NotNull(@this, "@this");

            TResult result = default(TResult);
            Exception exception = null;
            bool executed = false;

            return () =>
            {
                if (!executed)
                {
                    try
                    {
                        result = @this();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                        throw;
                    }
                    finally
                    {
                        @this = null;
                        executed = true;
                    }
                }

                if (exception != null)
                    exception.ThrowAsInnerExceptionIfNeeded();

                return result;
            };
        }

        public static Func<TResult> Synchronize<TResult>(this Func<TResult> @this)
        {
            return @this.Synchronize(() => true);
        }

        public static Func<TResult> Synchronize<TResult>(this Func<TResult> @this, Func<bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<TResult> Synchronize<TResult>(this Func<TResult> @this, Func<bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return () =>
            {
                if (needsSynchronizationPredicate())
                {
                    lock (gate)
                    {
                        return @this();
                    }
                }

                return @this();
            };
        }

        public static Func<Maybe<TResult>> Or<TResult>(this Func<Maybe<TResult>> @this, Func<Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return () => @this().Or(orFunc);
        }

        private class FuncComparer<T> : IComparer<T>
        {
            private readonly Func<T, T, int> _Strategy;

            public FuncComparer(Func<T, T, int> strategy)
            {
                _Strategy = strategy;
            }

            public int Compare(T x, T y)
            {
                return _Strategy(x, y);
            }
        }

        private class FuncEqualityComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _Strategy;
            private readonly Func<T, int> _HashCodeStrategy;

            public FuncEqualityComparer(Func<T, T, bool> strategy, Func<T, int> hashCodeStategy)
            {
                _Strategy = strategy;
                _HashCodeStrategy = hashCodeStategy;
            }

            public bool Equals(T x, T y)
            {
                return _Strategy(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _HashCodeStrategy(obj);
            }
        }
    }
}
