

using System;
using System.Collections.Generic;
using System.Threading;

namespace iSynaptic.Commons
{
    public static partial class ActionExtensions
    {
        
        
        public static Action<T1> MakeConditional<T1>(this Action<T1> @this, Func<T1, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1> MakeConditional<T1>(this Action<T1> @this, Func<T1, bool> condition, Action<T1> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1) =>
            {
                if (condition(t1))
                    @this(t1);
                else if (falseAction != null)
                    falseAction(t1);
            };
        }

        public static Action<T1> FollowedBy<T1>(this Action<T1> @this, Action<T1> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1) =>
            {
                @this(t1);
                followedBy(t1);
            };
        }

        public static Action<T1> MakeIdempotent<T1>(this Action<T1> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1);
                    @this = null;
                }
            };
        }

        public static Action<T1> CatchExceptions<T1>(this Action<T1> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1> CatchExceptions<T1>(this Action<T1> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1) =>
            {
                Action innerAction = () => @this(t1);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T2, T1> Flip<T1, T2>(this Action<T1, T2> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t1) => @this(t1, t2);
        }

        public static Action<T2> Curry<T1, T2>(this Action<T1, T2> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2) => @this(t1, t2);
        }

        
        public static Action<T1, T2> MakeConditional<T1, T2>(this Action<T1, T2> @this, Func<T1, T2, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2> MakeConditional<T1, T2>(this Action<T1, T2> @this, Func<T1, T2, bool> condition, Action<T1, T2> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2) =>
            {
                if (condition(t1, t2))
                    @this(t1, t2);
                else if (falseAction != null)
                    falseAction(t1, t2);
            };
        }

        public static Action<T1, T2> FollowedBy<T1, T2>(this Action<T1, T2> @this, Action<T1, T2> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2) =>
            {
                @this(t1, t2);
                followedBy(t1, t2);
            };
        }

        public static Action<T1, T2> MakeIdempotent<T1, T2>(this Action<T1, T2> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2> CatchExceptions<T1, T2>(this Action<T1, T2> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2> CatchExceptions<T1, T2>(this Action<T1, T2> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2) =>
            {
                Action innerAction = () => @this(t1, t2);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T3, T2, T1> Flip<T1, T2, T3>(this Action<T1, T2, T3> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t3, t2, t1) => @this(t1, t2, t3);
        }

        public static Action<T2, T3> Curry<T1, T2, T3>(this Action<T1, T2, T3> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3) => @this(t1, t2, t3);
        }

        
        public static Action<T1, T2, T3> MakeConditional<T1, T2, T3>(this Action<T1, T2, T3> @this, Func<T1, T2, T3, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3> MakeConditional<T1, T2, T3>(this Action<T1, T2, T3> @this, Func<T1, T2, T3, bool> condition, Action<T1, T2, T3> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3) =>
            {
                if (condition(t1, t2, t3))
                    @this(t1, t2, t3);
                else if (falseAction != null)
                    falseAction(t1, t2, t3);
            };
        }

        public static Action<T1, T2, T3> FollowedBy<T1, T2, T3>(this Action<T1, T2, T3> @this, Action<T1, T2, T3> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3) =>
            {
                @this(t1, t2, t3);
                followedBy(t1, t2, t3);
            };
        }

        public static Action<T1, T2, T3> MakeIdempotent<T1, T2, T3>(this Action<T1, T2, T3> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3> CatchExceptions<T1, T2, T3>(this Action<T1, T2, T3> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3> CatchExceptions<T1, T2, T3>(this Action<T1, T2, T3> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3) =>
            {
                Action innerAction = () => @this(t1, t2, t3);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T4, T3, T2, T1> Flip<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t4, t3, t2, t1) => @this(t1, t2, t3, t4);
        }

        public static Action<T2, T3, T4> Curry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4) => @this(t1, t2, t3, t4);
        }

        
        public static Action<T1, T2, T3, T4> MakeConditional<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this, Func<T1, T2, T3, T4, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4> MakeConditional<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this, Func<T1, T2, T3, T4, bool> condition, Action<T1, T2, T3, T4> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4) =>
            {
                if (condition(t1, t2, t3, t4))
                    @this(t1, t2, t3, t4);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4);
            };
        }

        public static Action<T1, T2, T3, T4> FollowedBy<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this, Action<T1, T2, T3, T4> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4) =>
            {
                @this(t1, t2, t3, t4);
                followedBy(t1, t2, t3, t4);
            };
        }

        public static Action<T1, T2, T3, T4> MakeIdempotent<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4> CatchExceptions<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4> CatchExceptions<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5);
        }

        public static Action<T2, T3, T4, T5> Curry<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5) => @this(t1, t2, t3, t4, t5);
        }

        
        public static Action<T1, T2, T3, T4, T5> MakeConditional<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this, Func<T1, T2, T3, T4, T5, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5> MakeConditional<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this, Func<T1, T2, T3, T4, T5, bool> condition, Action<T1, T2, T3, T4, T5> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5) =>
            {
                if (condition(t1, t2, t3, t4, t5))
                    @this(t1, t2, t3, t4, t5);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5);
            };
        }

        public static Action<T1, T2, T3, T4, T5> FollowedBy<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this, Action<T1, T2, T3, T4, T5> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5) =>
            {
                @this(t1, t2, t3, t4, t5);
                followedBy(t1, t2, t3, t4, t5);
            };
        }

        public static Action<T1, T2, T3, T4, T5> MakeIdempotent<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5> CatchExceptions<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5> CatchExceptions<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6);
        }

        public static Action<T2, T3, T4, T5, T6> Curry<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6) => @this(t1, t2, t3, t4, t5, t6);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6> MakeConditional<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> @this, Func<T1, T2, T3, T4, T5, T6, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6> MakeConditional<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> @this, Func<T1, T2, T3, T4, T5, T6, bool> condition, Action<T1, T2, T3, T4, T5, T6> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6))
                    @this(t1, t2, t3, t4, t5, t6);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6> FollowedBy<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> @this, Action<T1, T2, T3, T4, T5, T6> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6) =>
            {
                @this(t1, t2, t3, t4, t5, t6);
                followedBy(t1, t2, t3, t4, t5, t6);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6> MakeIdempotent<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6> CatchExceptions<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6> CatchExceptions<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7);
        }

        public static Action<T2, T3, T4, T5, T6, T7> Curry<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7) => @this(t1, t2, t3, t4, t5, t6, t7);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7> MakeConditional<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> MakeConditional<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> @this, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7))
                    @this(t1, t2, t3, t4, t5, t6, t7);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> FollowedBy<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> @this, Action<T1, T2, T3, T4, T5, T6, T7> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7);
                followedBy(t1, t2, t3, t4, t5, t6, t7);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> CatchExceptions<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> CatchExceptions<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T8, T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8> Curry<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8) => @this(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8))
                    @this(t1, t2, t3, t4, t5, t6, t7, t8);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> @this, Action<T1, T2, T3, T4, T5, T6, T7, T8> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7, t8);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7, t8);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T9, T8, T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9))
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> @this, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T10, T9, T8, T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10))
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @this, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11))
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @this, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12))
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @this, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T13, T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t13, t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13))
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @this, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T14, T13, T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t14, t13, t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14))
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @this, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T15, T14, T13, T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t15, t14, t13, t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15))
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> @this, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        
        
        public static Action<T16, T15, T14, T13, T12, T11, T10, T9, T8, T7, T6, T5, T4, T3, T2, T1> Flip<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> @this)
        {
            Guard.NotNull(@this, "@this");
            return (t16, t15, t14, t13, t12, t11, t10, t9, t8, t7, t6, t5, t4, t3, t2, t1) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return (t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        
        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return MakeConditional(@this, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> falseAction)
        {
            Guard.NotNull(@this, "@this");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16))
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> @this, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
                {
                    @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                    @this = null;
                }
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> @this)
        {
            Guard.NotNull(@this, "@this");
            return @this.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                Action innerAction = () => @this(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

            }
}