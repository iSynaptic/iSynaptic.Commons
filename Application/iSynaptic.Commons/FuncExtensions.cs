// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace iSynaptic.Commons
{
    public static partial class FuncExtensions
    {
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
