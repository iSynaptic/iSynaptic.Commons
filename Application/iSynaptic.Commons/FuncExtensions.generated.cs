

using System;
using System.Collections.Generic;

using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons
{
    public static partial class FuncExtensions
    {
        
        
        public static Action<T1> ToAction<T1, TRet>(this Func<T1, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1) => @this(t1);
        }

        public static Func<T1, bool> And<T1>(this Func<T1, bool> @this, Func<T1, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1) => @this(t1) && right(t1);
        }

        public static Func<T1, bool> Or<T1>(this Func<T1, bool> @this, Func<T1, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1) => @this(t1) || right(t1);
        }

        public static Func<T1, bool> XOr<T1>(this Func<T1, bool> @this, Func<T1, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1) => @this(t1) ^ right(t1);
        }

        public static Func<T1, TResult> MakeConditional<T1, TResult>(this Func<T1, TResult> @this, Func<T1, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, TResult> MakeConditional<T1, TResult>(this Func<T1, TResult> @this, Func<T1, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1) => defaultValue);
        }

        public static Func<T1, TResult> MakeConditional<T1, TResult>(this Func<T1, TResult> @this, Func<T1, bool> condition, Func<T1, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1) =>
            {
                if (condition(t1))
                    return @this(t1);

                if (falseFunc != null)
                    return falseFunc(t1);
                
                return default(TResult);
            };
        }

        public static Func<T1, Maybe<TResult>> Or<T1, TResult>(this Func<T1, Maybe<TResult>> @this, Func<T1, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1) =>
            {
                var results = @this(t1);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1);

                return results;
            };
        }

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

        
        
        public static Func<T2, T1, TRet> Flip<T1, T2, TRet>(this Func<T1, T2, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t1) => @this(t1, t2);
        }

        public static Func<T2, TRet> Curry<T1, T2, TRet>(this Func<T1, T2, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2) => @this(t1, t2);
        }

        
        public static Action<T1, T2> ToAction<T1, T2, TRet>(this Func<T1, T2, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2) => @this(t1, t2);
        }

        public static Func<T1, T2, bool> And<T1, T2>(this Func<T1, T2, bool> @this, Func<T1, T2, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2) => @this(t1, t2) && right(t1, t2);
        }

        public static Func<T1, T2, bool> Or<T1, T2>(this Func<T1, T2, bool> @this, Func<T1, T2, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2) => @this(t1, t2) || right(t1, t2);
        }

        public static Func<T1, T2, bool> XOr<T1, T2>(this Func<T1, T2, bool> @this, Func<T1, T2, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2) => @this(t1, t2) ^ right(t1, t2);
        }

        public static Func<T1, T2, TResult> MakeConditional<T1, T2, TResult>(this Func<T1, T2, TResult> @this, Func<T1, T2, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, TResult> MakeConditional<T1, T2, TResult>(this Func<T1, T2, TResult> @this, Func<T1, T2, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2) => defaultValue);
        }

        public static Func<T1, T2, TResult> MakeConditional<T1, T2, TResult>(this Func<T1, T2, TResult> @this, Func<T1, T2, bool> condition, Func<T1, T2, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2) =>
            {
                if (condition(t1, t2))
                    return @this(t1, t2);

                if (falseFunc != null)
                    return falseFunc(t1, t2);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, Maybe<TResult>> Or<T1, T2, TResult>(this Func<T1, T2, Maybe<TResult>> @this, Func<T1, T2, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2) =>
            {
                var results = @this(t1, t2);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2);

                return results;
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

        
        
        public static Func<T3, T2, T1, TRet> Flip<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t3, t2, t1) => @this(t1, t2, t3);
        }

        public static Func<T2, T3, TRet> Curry<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3) => @this(t1, t2, t3);
        }

        
        public static Action<T1, T2, T3> ToAction<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3) => @this(t1, t2, t3);
        }

        public static Func<T1, T2, T3, bool> And<T1, T2, T3>(this Func<T1, T2, T3, bool> @this, Func<T1, T2, T3, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3) => @this(t1, t2, t3) && right(t1, t2, t3);
        }

        public static Func<T1, T2, T3, bool> Or<T1, T2, T3>(this Func<T1, T2, T3, bool> @this, Func<T1, T2, T3, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3) => @this(t1, t2, t3) || right(t1, t2, t3);
        }

        public static Func<T1, T2, T3, bool> XOr<T1, T2, T3>(this Func<T1, T2, T3, bool> @this, Func<T1, T2, T3, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3) => @this(t1, t2, t3) ^ right(t1, t2, t3);
        }

        public static Func<T1, T2, T3, TResult> MakeConditional<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @this, Func<T1, T2, T3, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, TResult> MakeConditional<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @this, Func<T1, T2, T3, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3) => defaultValue);
        }

        public static Func<T1, T2, T3, TResult> MakeConditional<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> @this, Func<T1, T2, T3, bool> condition, Func<T1, T2, T3, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3) =>
            {
                if (condition(t1, t2, t3))
                    return @this(t1, t2, t3);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, Maybe<TResult>> Or<T1, T2, T3, TResult>(this Func<T1, T2, T3, Maybe<TResult>> @this, Func<T1, T2, T3, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3) =>
            {
                var results = @this(t1, t2, t3);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3);

                return results;
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

        
        
        public static Func<T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t4, t3, t2, t1) => @this(t1, t2, t3, t4);
        }

        public static Func<T2, T3, T4, TRet> Curry<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4) => @this(t1, t2, t3, t4);
        }

        
        public static Action<T1, T2, T3, T4> ToAction<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4) => @this(t1, t2, t3, t4);
        }

        public static Func<T1, T2, T3, T4, bool> And<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> @this, Func<T1, T2, T3, T4, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4) => @this(t1, t2, t3, t4) && right(t1, t2, t3, t4);
        }

        public static Func<T1, T2, T3, T4, bool> Or<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> @this, Func<T1, T2, T3, T4, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4) => @this(t1, t2, t3, t4) || right(t1, t2, t3, t4);
        }

        public static Func<T1, T2, T3, T4, bool> XOr<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> @this, Func<T1, T2, T3, T4, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4) => @this(t1, t2, t3, t4) ^ right(t1, t2, t3, t4);
        }

        public static Func<T1, T2, T3, T4, TResult> MakeConditional<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @this, Func<T1, T2, T3, T4, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, TResult> MakeConditional<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @this, Func<T1, T2, T3, T4, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, TResult> MakeConditional<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> @this, Func<T1, T2, T3, T4, bool> condition, Func<T1, T2, T3, T4, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4) =>
            {
                if (condition(t1, t2, t3, t4))
                    return @this(t1, t2, t3, t4);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, Maybe<TResult>> Or<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, Maybe<TResult>> @this, Func<T1, T2, T3, T4, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4) =>
            {
                var results = @this(t1, t2, t3, t4);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4);

                return results;
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

        
        
        public static Func<T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, TRet>(this Func<T1, T2, T3, T4, T5, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5);
        }

        public static Func<T2, T3, T4, T5, TRet> Curry<T1, T2, T3, T4, T5, TRet>(this Func<T1, T2, T3, T4, T5, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);
        }

        
        public static Action<T1, T2, T3, T4, T5> ToAction<T1, T2, T3, T4, T5, TRet>(this Func<T1, T2, T3, T4, T5, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);
        }

        public static Func<T1, T2, T3, T4, T5, bool> And<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> @this, Func<T1, T2, T3, T4, T5, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5) && right(t1, t2, t3, t4, t5);
        }

        public static Func<T1, T2, T3, T4, T5, bool> Or<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> @this, Func<T1, T2, T3, T4, T5, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5) || right(t1, t2, t3, t4, t5);
        }

        public static Func<T1, T2, T3, T4, T5, bool> XOr<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> @this, Func<T1, T2, T3, T4, T5, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5) ^ right(t1, t2, t3, t4, t5);
        }

        public static Func<T1, T2, T3, T4, T5, TResult> MakeConditional<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> @this, Func<T1, T2, T3, T4, T5, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, TResult> MakeConditional<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> @this, Func<T1, T2, T3, T4, T5, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, TResult> MakeConditional<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> @this, Func<T1, T2, T3, T4, T5, bool> condition, Func<T1, T2, T3, T4, T5, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5) =>
            {
                if (condition(t1, t2, t3, t4, t5))
                    return @this(t1, t2, t3, t4, t5);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, Maybe<TResult>> Or<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5) =>
            {
                var results = @this(t1, t2, t3, t4, t5);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5);

                return results;
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

        
        
        public static Func<T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, TRet>(this Func<T1, T2, T3, T4, T5, T6, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6);
        }

        public static Func<T2, T3, T4, T5, T6, TRet> Curry<T1, T2, T3, T4, T5, T6, TRet>(this Func<T1, T2, T3, T4, T5, T6, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6> ToAction<T1, T2, T3, T4, T5, T6, TRet>(this Func<T1, T2, T3, T4, T5, T6, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6);
        }

        public static Func<T1, T2, T3, T4, T5, T6, bool> And<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6, bool> @this, Func<T1, T2, T3, T4, T5, T6, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6) && right(t1, t2, t3, t4, t5, t6);
        }

        public static Func<T1, T2, T3, T4, T5, T6, bool> Or<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6, bool> @this, Func<T1, T2, T3, T4, T5, T6, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6) || right(t1, t2, t3, t4, t5, t6);
        }

        public static Func<T1, T2, T3, T4, T5, T6, bool> XOr<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6, bool> @this, Func<T1, T2, T3, T4, T5, T6, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6) ^ right(t1, t2, t3, t4, t5, t6);
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> @this, Func<T1, T2, T3, T4, T5, T6, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> @this, Func<T1, T2, T3, T4, T5, T6, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> @this, Func<T1, T2, T3, T4, T5, T6, bool> condition, Func<T1, T2, T3, T4, T5, T6, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6))
                    return @this(t1, t2, t3, t4, t5, t6);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6);

                return results;
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

        
        
        public static Func<T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7);
        }

        public static Func<T2, T3, T4, T5, T6, T7, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7> ToAction<T1, T2, T3, T4, T5, T6, T7, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, bool> And<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7) && right(t1, t2, t3, t4, t5, t6, t7);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, bool> Or<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7) || right(t1, t2, t3, t4, t5, t6, t7);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, bool> XOr<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7) ^ right(t1, t2, t3, t4, t5, t6, t7);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7))
                    return @this(t1, t2, t3, t4, t5, t6, t7);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7);

                return results;
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

        
        
        public static Func<T8, T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> And<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8) && right(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8) || right(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8) ^ right(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7, t8) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8))
                    return @this(t1, t2, t3, t4, t5, t6, t7, t8);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7, t8);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7, t8);

                return results;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7, t8);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7, t8);
            };
        }

        
        
        public static Func<T9, T8, T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9))
                    return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9);

                return results;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
            };
        }

        
        
        public static Func<T10, T9, T8, T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10))
                    return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);

                return results;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            };
        }

        
        
        public static Func<T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11))
                    return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);

                return results;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            };
        }

        
        
        public static Func<T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12))
                    return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);

                return results;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            };
        }

        
        
        public static Func<T13, T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t13, t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13))
                    return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);

                return results;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            };
        }

        
        
        public static Func<T14, T13, T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t14, t13, t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14))
                    return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);

                return results;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            };
        }

        
        
        public static Func<T15, T14, T13, T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t15, t14, t13, t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15))
                    return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);

                return results;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            };
        }

        
        
        public static Func<T16, T15, T14, T13, T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1, TRet> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t16, t15, t14, t13, t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRet> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRet> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRet> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> right)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition)
        {
            return MakeConditional(@this, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition, TResult defaultValue)
        {
            return MakeConditional(@this, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> falseFunc)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16))
                    return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                
                return default(TResult);
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Maybe<TResult>> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Maybe<TResult>> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Maybe<TResult>> orFunc)
        {
            if (@this == null || orFunc == null)
                return @this ?? orFunc;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                var results = @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);

                if(results.HasValue != true && results.Exception == null)
                    return orFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);

                return results;
            };
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> @this)
        {
            return @this.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            return Synchronize(@this, needsSynchronizationPredicate, new object());
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> needsSynchronizationPredicate, object gate)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");
            Guard.NotNull(gate, "gate");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16))
                {
                    lock (gate)
                    {
                        return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                    }
                }

                return @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
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
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7, t8) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8>(t8))];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9>>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9>(t8, t9))];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10>>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10>(t8, t9, t10))];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11>>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11>(t8, t9, t10, t11))];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12>>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12>(t8, t9, t10, t11, t12))];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13>>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5, x.Rest.Item6).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12, T13>(t8, t9, t10, t11, t12, t13))];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14>>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5, x.Rest.Item6, x.Rest.Item7).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12, T13, T14>(t8, t9, t10, t11, t12, t13, t14))];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15>>>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5, x.Rest.Item6, x.Rest.Item7, x.Rest.Rest.Item1).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15>>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15>>(t8, t9, t10, t11, t12, t13, t14, new Tuple<T15>(t15)))];
        }
        
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> @this)
        {
            Guard.NotNull(@this, "@this");
            var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16>>>, TResult>(x => @this(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5, x.Rest.Item6, x.Rest.Item7, x.Rest.Rest.Item1, x.Rest.Rest.Item2).ToMaybe());

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16>>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16>>(t8, t9, t10, t11, t12, t13, t14, new Tuple<T15, T16>(t15, t16)))];
        }
            }
}
