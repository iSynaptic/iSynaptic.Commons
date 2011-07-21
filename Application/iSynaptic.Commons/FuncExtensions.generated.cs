// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
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

using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons
{
    public static partial class FuncExtensions
    {
        
        public static Func<T1, TResult> Synchronize<T1, TResult>(this Func<T1, TResult> @this)
        {
            return @this.Synchronize((t1) => true);
        }

        public static Func<T1, TResult> Synchronize<T1, TResult>(this Func<T1, TResult> @this, Func<T1, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, TResult> Synchronize<T1, TResult>(this Func<T1, TResult> @this, Func<T1, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1) =>
            {
                if(needsSynchronizationPredicate(t1))
                {
                    lock (gate)
                    {
                        return @this(t1);
                    }
                }

                return @this(t1);
            };
        }

        
        public static Func<T1, T2, TResult> Synchronize<T1, T2, TResult>(this Func<T1, T2, TResult> @this)
        {
            return @this.Synchronize((t1, t2) => true);
        }

        public static Func<T1, T2, TResult> Synchronize<T1, T2, TResult>(this Func<T1, T2, TResult> @this, Func<T1, T2, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, TResult> Synchronize<T1, T2, TResult>(this Func<T1, T2, TResult> @this, Func<T1, T2, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2) =>
            {
                if(needsSynchronizationPredicate(t1, t2))
                {
                    lock (gate)
                    {
                        return @this(t1, t2);
                    }
                }

                return @this(t1, t2);
            };
        }

        
        public static Func<T1, T2, T3, TResult> Synchronize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3) => true);
        }

        public static Func<T1, T2, T3, TResult> Synchronize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @this, Func<T1, T2, T3, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, TResult> Synchronize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @this, Func<T1, T2, T3, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3);
                    }
                }

                return @this(t1, t2, t3);
            };
        }

        
        public static Func<T1, T2, T3, T4, TResult> Synchronize<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4) => true);
        }

        public static Func<T1, T2, T3, T4, TResult> Synchronize<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @this, Func<T1, T2, T3, T4, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, TResult> Synchronize<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @this, Func<T1, T2, T3, T4, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4);
                    }
                }

                return @this(t1, t2, t3, t4);
            };
        }

        
        public static Func<T1, T2, T3, T4, T5, TResult> Synchronize<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5) => true);
        }

        public static Func<T1, T2, T3, T4, T5, TResult> Synchronize<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> @this, Func<T1, T2, T3, T4, T5, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, TResult> Synchronize<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> @this, Func<T1, T2, T3, T4, T5, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5);
                    }
                }

                return @this(t1, t2, t3, t4, t5);
            };
        }

        
        public static Func<T1, T2, T3, T4, T5, T6, TResult> Synchronize<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> Synchronize<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> @this, Func<T1, T2, T3, T4, T5, T6, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> Synchronize<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> @this, Func<T1, T2, T3, T4, T5, T6, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6);
            };
        }

        
        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7);
            };
        }

        
        
        public static Func<T1, TResult> Memoize<T1, TResult>(this Func<T1, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1>, TResult>(x => @this(x.Item1).ToMaybe());

            return (t1) => dictionary[new Tuple<T1>(t1)];
        }
        
        public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2>, TResult>(x => @this(x.Item1, x.Item2).ToMaybe());

            return (t1, t2) => dictionary[new Tuple<T1, T2>(t1, t2)];
        }
        
        public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3>, TResult>(x => @this(x.Item1, x.Item2, x.Item3).ToMaybe());

            return (t1, t2, t3) => dictionary[new Tuple<T1, T2, T3>(t1, t2, t3)];
        }
        
        public static Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4).ToMaybe());

            return (t1, t2, t3, t4) => dictionary[new Tuple<T1, T2, T3, T4>(t1, t2, t3, t4)];
        }
        
        public static Func<T1, T2, T3, T4, T5, TResult> Memoize<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5).ToMaybe());

            return (t1, t2, t3, t4, t5) => dictionary[new Tuple<T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5)];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, TResult> Memoize<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6).ToMaybe());

            return (t1, t2, t3, t4, t5, t6) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6>(t1, t2, t3, t4, t5, t6)];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7>(t1, t2, t3, t4, t5, t6, t7)];
        }
            }
}
