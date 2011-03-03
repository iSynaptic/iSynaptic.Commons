using System;
using System.Collections.Generic;
using System.Threading;

namespace iSynaptic.Commons
{
    public static partial class FuncExtensions
    {
        public static Action ToAction<TRet>(this Func<TRet> self)
        {
            Guard.NotNull(self, "self");
            return () => self();
        }

        public static Func<T, bool> And<T>(this Func<T, bool> self, Func<T, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return input => self(input) && right(input);
        }

        public static Func<T, bool> Or<T>(this Func<T, bool> self, Func<T, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return input => self(input) || right(input);
        }

        public static Func<T, bool> XOr<T>(this Func<T, bool> self, Func<T, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return input => self(input) ^ right(input);
        }

        public static IComparer<T> ToComparer<T>(this Func<T, T, int> self)
        {
            Guard.NotNull(self, "self");
            return new FuncComparer<T>(self);
        }

        public static Func<TResult> Memoize<TResult>(this Func<TResult> self)
        {
            Guard.NotNull(self, "self");

            TResult result = default(TResult);
            var spinLock = new SpinLock();
            bool executed = false;

            return () =>
            {
                if(!executed)
                {
                    bool lockTaken = false;
                    try
                    {
                        spinLock.Enter(ref lockTaken);
                        if (!executed)
                            result = self();
                    }
                    finally
                    {
                        executed = true;
                        if(lockTaken)
                            spinLock.Exit();
                    }
                }

                return result;
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

    }
}
