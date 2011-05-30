using System;
using System.Collections.Generic;
using System.Threading;
using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons
{
    public static partial class FuncExtensions
    {
        public static Action ToAction<TRet>(this Func<TRet> self)
        {
            Guard.NotNull(self, "self");
            return () => self();
        }

        public static IComparer<T> ToComparer<T>(this Func<T, T, int> self)
        {
            Guard.NotNull(self, "self");
            return new FuncComparer<T>(self);
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

        public static Func<TResult> Memoize<TResult>(this Func<TResult> self)
        {
            Guard.NotNull(self, "self");

            TResult result = default(TResult);
            Exception exception = null;
            bool executed = false;

            return () =>
            {
                if (!executed)
                {
                    try
                    {
                        result = self();
                    }
                    catch (Exception ex)
                    {
                        exception = ex;
                        throw;
                    }
                    finally
                    {
                        self = null;
                        executed = true;
                    }
                }

                if (exception != null)
                    throw exception;

                return result;
            };
        }

        public static Func<TResult> Synchronize<TResult>(this Func<TResult> self)
        {
            return self.Synchronize(() => true);
        }

        public static Func<TResult> Synchronize<TResult>(this Func<TResult> self, Func<bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();
            return SynchronizeWith(self, needsSynchronizationPredicate, lockObject);
        }

        public static Func<TResult> SynchronizeWith<TResult>(this Func<TResult> self, Func<bool> needsSynchronizationPredicate, object lockObject)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(lockObject, "lockObject");

            return () =>
            {
                if (needsSynchronizationPredicate())
                {
                    lock (lockObject)
                    {
                        return self();
                    }
                }

                return self();
            };
        }

        public static Func<Maybe<TResult>> Or<TResult>(this Func<Maybe<TResult>> self, Func<Maybe<TResult>> orFunc)
        {
            if (self == null || orFunc == null)
                return self ?? orFunc;

            return () =>
            {
                var results = self();

                if (results.HasValue != true && results.Exception == null)
                    return orFunc();

                return results;
            };
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
