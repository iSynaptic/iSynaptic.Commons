

using System;
using System.Collections.Generic;
using System.Threading;

namespace iSynaptic.Commons
{
	public static partial class ActionExtensions
	{
		
		public static Action<T1> MakeConditional<T1>(this Action<T1> self, Func<T1, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1> MakeConditional<T1>(this Action<T1> self, Func<T1, bool> condition, Action<T1> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1) =>
            {
                if (condition(t1))
                    self(t1);
                else if (falseAction != null)
                    falseAction(t1);
            };
        }

		public static Action<T1> FollowedBy<T1>(this Action<T1> self, Action<T1> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1) =>
            {
                self(t1);
                followedBy(t1);
            };
        }

        public static Action<T1> MakeIdempotent<T1>(this Action<T1> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1);
					self = null;
				}
            };
        }

		public static Action<T1> CatchExceptions<T1>(this Action<T1> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1> CatchExceptions<T1>(this Action<T1> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1) =>
            {
                Action innerAction = () => self(t1);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2> MakeConditional<T1, T2>(this Action<T1, T2> self, Func<T1, T2, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2> MakeConditional<T1, T2>(this Action<T1, T2> self, Func<T1, T2, bool> condition, Action<T1, T2> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2) =>
            {
                if (condition(t1, t2))
                    self(t1, t2);
                else if (falseAction != null)
                    falseAction(t1, t2);
            };
        }

		public static Action<T1, T2> FollowedBy<T1, T2>(this Action<T1, T2> self, Action<T1, T2> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2) =>
            {
                self(t1, t2);
                followedBy(t1, t2);
            };
        }

        public static Action<T1, T2> MakeIdempotent<T1, T2>(this Action<T1, T2> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2);
					self = null;
				}
            };
        }

		public static Action<T1, T2> CatchExceptions<T1, T2>(this Action<T1, T2> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2> CatchExceptions<T1, T2>(this Action<T1, T2> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2) =>
            {
                Action innerAction = () => self(t1, t2);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3> MakeConditional<T1, T2, T3>(this Action<T1, T2, T3> self, Func<T1, T2, T3, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3> MakeConditional<T1, T2, T3>(this Action<T1, T2, T3> self, Func<T1, T2, T3, bool> condition, Action<T1, T2, T3> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3) =>
            {
                if (condition(t1, t2, t3))
                    self(t1, t2, t3);
                else if (falseAction != null)
                    falseAction(t1, t2, t3);
            };
        }

		public static Action<T1, T2, T3> FollowedBy<T1, T2, T3>(this Action<T1, T2, T3> self, Action<T1, T2, T3> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3) =>
            {
                self(t1, t2, t3);
                followedBy(t1, t2, t3);
            };
        }

        public static Action<T1, T2, T3> MakeIdempotent<T1, T2, T3>(this Action<T1, T2, T3> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3> CatchExceptions<T1, T2, T3>(this Action<T1, T2, T3> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3> CatchExceptions<T1, T2, T3>(this Action<T1, T2, T3> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3) =>
            {
                Action innerAction = () => self(t1, t2, t3);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4> MakeConditional<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> self, Func<T1, T2, T3, T4, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4> MakeConditional<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> self, Func<T1, T2, T3, T4, bool> condition, Action<T1, T2, T3, T4> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4) =>
            {
                if (condition(t1, t2, t3, t4))
                    self(t1, t2, t3, t4);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4);
            };
        }

		public static Action<T1, T2, T3, T4> FollowedBy<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> self, Action<T1, T2, T3, T4> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4) =>
            {
                self(t1, t2, t3, t4);
                followedBy(t1, t2, t3, t4);
            };
        }

        public static Action<T1, T2, T3, T4> MakeIdempotent<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4> CatchExceptions<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4> CatchExceptions<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5> MakeConditional<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> self, Func<T1, T2, T3, T4, T5, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5> MakeConditional<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> self, Func<T1, T2, T3, T4, T5, bool> condition, Action<T1, T2, T3, T4, T5> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5) =>
            {
                if (condition(t1, t2, t3, t4, t5))
                    self(t1, t2, t3, t4, t5);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5);
            };
        }

		public static Action<T1, T2, T3, T4, T5> FollowedBy<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> self, Action<T1, T2, T3, T4, T5> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5) =>
            {
                self(t1, t2, t3, t4, t5);
                followedBy(t1, t2, t3, t4, t5);
            };
        }

        public static Action<T1, T2, T3, T4, T5> MakeIdempotent<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5> CatchExceptions<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5> CatchExceptions<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6> MakeConditional<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> self, Func<T1, T2, T3, T4, T5, T6, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6> MakeConditional<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> self, Func<T1, T2, T3, T4, T5, T6, bool> condition, Action<T1, T2, T3, T4, T5, T6> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6))
                    self(t1, t2, t3, t4, t5, t6);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6> FollowedBy<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> self, Action<T1, T2, T3, T4, T5, T6> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6) =>
            {
                self(t1, t2, t3, t4, t5, t6);
                followedBy(t1, t2, t3, t4, t5, t6);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6> MakeIdempotent<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6> CatchExceptions<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6> CatchExceptions<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7> MakeConditional<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> self, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> MakeConditional<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> self, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7))
                    self(t1, t2, t3, t4, t5, t6, t7);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7> FollowedBy<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> self, Action<T1, T2, T3, T4, T5, T6, T7> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7);
                followedBy(t1, t2, t3, t4, t5, t6, t7);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7> CatchExceptions<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7> CatchExceptions<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8))
                    self(t1, t2, t3, t4, t5, t6, t7, t8);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> self, Action<T1, T2, T3, T4, T5, T6, T7, T8> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7, t8);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7, t8);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7, t8);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9))
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10))
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11))
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12))
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13))
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14))
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15))
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> falseAction)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16))
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                else if (falseAction != null)
                    falseAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> self, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            };
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> MakeIdempotent<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if(previousValue == 0)
				{
                    self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
					self = null;
				}
            };
        }

		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> CatchExceptions<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                Action innerAction = () => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

			}
}