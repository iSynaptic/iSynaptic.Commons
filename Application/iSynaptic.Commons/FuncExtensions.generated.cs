

using System;
using System.Collections.Generic;

using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons
{
	public static partial class FuncExtensions
	{
		
		public static Action<T1> ToAction<T1, TRet>(this Func<T1, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1) => self(t1);
        }

		public static Func<T1, bool> And<T1>(this Func<T1, bool> self, Func<T1, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1) => self(t1) && right(t1);
        }

        public static Func<T1, bool> Or<T1>(this Func<T1, bool> self, Func<T1, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1) => self(t1) || right(t1);
        }

        public static Func<T1, bool> XOr<T1>(this Func<T1, bool> self, Func<T1, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1) => self(t1) ^ right(t1);
        }

		public static Func<T1, TResult> MakeConditional<T1, TResult>(this Func<T1, TResult> self, Func<T1, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, TResult> MakeConditional<T1, TResult>(this Func<T1, TResult> self, Func<T1, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1) => defaultValue);
        }

        public static Func<T1, TResult> MakeConditional<T1, TResult>(this Func<T1, TResult> self, Func<T1, bool> condition, Func<T1, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1) =>
            {
                if (condition(t1))
                    return self(t1);

                if (falseFunc != null)
                    return falseFunc(t1);
                
                return default(TResult);
            };
        }

		public static Func<T1, Maybe<TResult>> PrecededBy<T1, TResult>(this Func<T1, Maybe<TResult>> self, Func<T1, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1) => precededBy(t1).Or(self(t1));
        }

        public static Func<T1, Maybe<TResult>> FollowedBy<T1, TResult>(this Func<T1, Maybe<TResult>> self, Func<T1, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1) => self(t1).Or(followedBy(t1));
        }

		public static Func<T1, TResult> Synchronize<T1, TResult>(this Func<T1, TResult> self)
        {
            return self.Synchronize((t1) => true);
        }

        public static Func<T1, TResult> Synchronize<T1, TResult>(this Func<T1, TResult> self, Func<T1, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1) =>
            {
                if(needsSynchronizationPredicate(t1))
                {
                    lock (lockObject)
                    {
                        return self(t1);
                    }
                }

                return self(t1);
            };
        }

		
		public static Action<T1, T2> ToAction<T1, T2, TRet>(this Func<T1, T2, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2) => self(t1, t2);
        }

		public static Func<T1, T2, bool> And<T1, T2>(this Func<T1, T2, bool> self, Func<T1, T2, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2) => self(t1, t2) && right(t1, t2);
        }

        public static Func<T1, T2, bool> Or<T1, T2>(this Func<T1, T2, bool> self, Func<T1, T2, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2) => self(t1, t2) || right(t1, t2);
        }

        public static Func<T1, T2, bool> XOr<T1, T2>(this Func<T1, T2, bool> self, Func<T1, T2, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2) => self(t1, t2) ^ right(t1, t2);
        }

		public static Func<T1, T2, TResult> MakeConditional<T1, T2, TResult>(this Func<T1, T2, TResult> self, Func<T1, T2, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, TResult> MakeConditional<T1, T2, TResult>(this Func<T1, T2, TResult> self, Func<T1, T2, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2) => defaultValue);
        }

        public static Func<T1, T2, TResult> MakeConditional<T1, T2, TResult>(this Func<T1, T2, TResult> self, Func<T1, T2, bool> condition, Func<T1, T2, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2) =>
            {
                if (condition(t1, t2))
                    return self(t1, t2);

                if (falseFunc != null)
                    return falseFunc(t1, t2);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, Maybe<TResult>> PrecededBy<T1, T2, TResult>(this Func<T1, T2, Maybe<TResult>> self, Func<T1, T2, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2) => precededBy(t1, t2).Or(self(t1, t2));
        }

        public static Func<T1, T2, Maybe<TResult>> FollowedBy<T1, T2, TResult>(this Func<T1, T2, Maybe<TResult>> self, Func<T1, T2, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2) => self(t1, t2).Or(followedBy(t1, t2));
        }

		public static Func<T1, T2, TResult> Synchronize<T1, T2, TResult>(this Func<T1, T2, TResult> self)
        {
            return self.Synchronize((t1, t2) => true);
        }

        public static Func<T1, T2, TResult> Synchronize<T1, T2, TResult>(this Func<T1, T2, TResult> self, Func<T1, T2, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2) =>
            {
                if(needsSynchronizationPredicate(t1, t2))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2);
                    }
                }

                return self(t1, t2);
            };
        }

		
		public static Action<T1, T2, T3> ToAction<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3) => self(t1, t2, t3);
        }

		public static Func<T1, T2, T3, bool> And<T1, T2, T3>(this Func<T1, T2, T3, bool> self, Func<T1, T2, T3, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3) => self(t1, t2, t3) && right(t1, t2, t3);
        }

        public static Func<T1, T2, T3, bool> Or<T1, T2, T3>(this Func<T1, T2, T3, bool> self, Func<T1, T2, T3, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3) => self(t1, t2, t3) || right(t1, t2, t3);
        }

        public static Func<T1, T2, T3, bool> XOr<T1, T2, T3>(this Func<T1, T2, T3, bool> self, Func<T1, T2, T3, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3) => self(t1, t2, t3) ^ right(t1, t2, t3);
        }

		public static Func<T1, T2, T3, TResult> MakeConditional<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> self, Func<T1, T2, T3, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, TResult> MakeConditional<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> self, Func<T1, T2, T3, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3) => defaultValue);
        }

        public static Func<T1, T2, T3, TResult> MakeConditional<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> self, Func<T1, T2, T3, bool> condition, Func<T1, T2, T3, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3) =>
            {
                if (condition(t1, t2, t3))
                    return self(t1, t2, t3);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, Maybe<TResult>> PrecededBy<T1, T2, T3, TResult>(this Func<T1, T2, T3, Maybe<TResult>> self, Func<T1, T2, T3, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3) => precededBy(t1, t2, t3).Or(self(t1, t2, t3));
        }

        public static Func<T1, T2, T3, Maybe<TResult>> FollowedBy<T1, T2, T3, TResult>(this Func<T1, T2, T3, Maybe<TResult>> self, Func<T1, T2, T3, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3) => self(t1, t2, t3).Or(followedBy(t1, t2, t3));
        }

		public static Func<T1, T2, T3, TResult> Synchronize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> self)
        {
            return self.Synchronize((t1, t2, t3) => true);
        }

        public static Func<T1, T2, T3, TResult> Synchronize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> self, Func<T1, T2, T3, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3);
                    }
                }

                return self(t1, t2, t3);
            };
        }

		
		public static Action<T1, T2, T3, T4> ToAction<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4) => self(t1, t2, t3, t4);
        }

		public static Func<T1, T2, T3, T4, bool> And<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> self, Func<T1, T2, T3, T4, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4) => self(t1, t2, t3, t4) && right(t1, t2, t3, t4);
        }

        public static Func<T1, T2, T3, T4, bool> Or<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> self, Func<T1, T2, T3, T4, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4) => self(t1, t2, t3, t4) || right(t1, t2, t3, t4);
        }

        public static Func<T1, T2, T3, T4, bool> XOr<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, bool> self, Func<T1, T2, T3, T4, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4) => self(t1, t2, t3, t4) ^ right(t1, t2, t3, t4);
        }

		public static Func<T1, T2, T3, T4, TResult> MakeConditional<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> self, Func<T1, T2, T3, T4, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, TResult> MakeConditional<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> self, Func<T1, T2, T3, T4, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, TResult> MakeConditional<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> self, Func<T1, T2, T3, T4, bool> condition, Func<T1, T2, T3, T4, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4) =>
            {
                if (condition(t1, t2, t3, t4))
                    return self(t1, t2, t3, t4);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, Maybe<TResult>> self, Func<T1, T2, T3, T4, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4) => precededBy(t1, t2, t3, t4).Or(self(t1, t2, t3, t4));
        }

        public static Func<T1, T2, T3, T4, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, Maybe<TResult>> self, Func<T1, T2, T3, T4, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4) => self(t1, t2, t3, t4).Or(followedBy(t1, t2, t3, t4));
        }

		public static Func<T1, T2, T3, T4, TResult> Synchronize<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4) => true);
        }

        public static Func<T1, T2, T3, T4, TResult> Synchronize<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> self, Func<T1, T2, T3, T4, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4);
                    }
                }

                return self(t1, t2, t3, t4);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5> ToAction<T1, T2, T3, T4, T5, TRet>(this Func<T1, T2, T3, T4, T5, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5) => self(t1, t2, t3, t4, t5);
        }

		public static Func<T1, T2, T3, T4, T5, bool> And<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> self, Func<T1, T2, T3, T4, T5, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5) => self(t1, t2, t3, t4, t5) && right(t1, t2, t3, t4, t5);
        }

        public static Func<T1, T2, T3, T4, T5, bool> Or<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> self, Func<T1, T2, T3, T4, T5, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5) => self(t1, t2, t3, t4, t5) || right(t1, t2, t3, t4, t5);
        }

        public static Func<T1, T2, T3, T4, T5, bool> XOr<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, bool> self, Func<T1, T2, T3, T4, T5, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5) => self(t1, t2, t3, t4, t5) ^ right(t1, t2, t3, t4, t5);
        }

		public static Func<T1, T2, T3, T4, T5, TResult> MakeConditional<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> self, Func<T1, T2, T3, T4, T5, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, TResult> MakeConditional<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> self, Func<T1, T2, T3, T4, T5, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, TResult> MakeConditional<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> self, Func<T1, T2, T3, T4, T5, bool> condition, Func<T1, T2, T3, T4, T5, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5) =>
            {
                if (condition(t1, t2, t3, t4, t5))
                    return self(t1, t2, t3, t4, t5);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5) => precededBy(t1, t2, t3, t4, t5).Or(self(t1, t2, t3, t4, t5));
        }

        public static Func<T1, T2, T3, T4, T5, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5) => self(t1, t2, t3, t4, t5).Or(followedBy(t1, t2, t3, t4, t5));
        }

		public static Func<T1, T2, T3, T4, T5, TResult> Synchronize<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5) => true);
        }

        public static Func<T1, T2, T3, T4, T5, TResult> Synchronize<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> self, Func<T1, T2, T3, T4, T5, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5);
                    }
                }

                return self(t1, t2, t3, t4, t5);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6> ToAction<T1, T2, T3, T4, T5, T6, TRet>(this Func<T1, T2, T3, T4, T5, T6, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6) => self(t1, t2, t3, t4, t5, t6);
        }

		public static Func<T1, T2, T3, T4, T5, T6, bool> And<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6, bool> self, Func<T1, T2, T3, T4, T5, T6, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6) => self(t1, t2, t3, t4, t5, t6) && right(t1, t2, t3, t4, t5, t6);
        }

        public static Func<T1, T2, T3, T4, T5, T6, bool> Or<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6, bool> self, Func<T1, T2, T3, T4, T5, T6, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6) => self(t1, t2, t3, t4, t5, t6) || right(t1, t2, t3, t4, t5, t6);
        }

        public static Func<T1, T2, T3, T4, T5, T6, bool> XOr<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6, bool> self, Func<T1, T2, T3, T4, T5, T6, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6) => self(t1, t2, t3, t4, t5, t6) ^ right(t1, t2, t3, t4, t5, t6);
        }

		public static Func<T1, T2, T3, T4, T5, T6, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> self, Func<T1, T2, T3, T4, T5, T6, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> self, Func<T1, T2, T3, T4, T5, T6, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> self, Func<T1, T2, T3, T4, T5, T6, bool> condition, Func<T1, T2, T3, T4, T5, T6, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6))
                    return self(t1, t2, t3, t4, t5, t6);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6) => precededBy(t1, t2, t3, t4, t5, t6).Or(self(t1, t2, t3, t4, t5, t6));
        }

        public static Func<T1, T2, T3, T4, T5, T6, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6) => self(t1, t2, t3, t4, t5, t6).Or(followedBy(t1, t2, t3, t4, t5, t6));
        }

		public static Func<T1, T2, T3, T4, T5, T6, TResult> Synchronize<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, TResult> Synchronize<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> self, Func<T1, T2, T3, T4, T5, T6, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7> ToAction<T1, T2, T3, T4, T5, T6, T7, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7) => self(t1, t2, t3, t4, t5, t6, t7);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, bool> And<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7) => self(t1, t2, t3, t4, t5, t6, t7) && right(t1, t2, t3, t4, t5, t6, t7);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, bool> Or<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7) => self(t1, t2, t3, t4, t5, t6, t7) || right(t1, t2, t3, t4, t5, t6, t7);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, bool> XOr<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7) => self(t1, t2, t3, t4, t5, t6, t7) ^ right(t1, t2, t3, t4, t5, t6, t7);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7))
                    return self(t1, t2, t3, t4, t5, t6, t7);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7) => precededBy(t1, t2, t3, t4, t5, t6, t7).Or(self(t1, t2, t3, t4, t5, t6, t7));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7) => self(t1, t2, t3, t4, t5, t6, t7).Or(followedBy(t1, t2, t3, t4, t5, t6, t7));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7, t8) => self(t1, t2, t3, t4, t5, t6, t7, t8);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> And<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8) => self(t1, t2, t3, t4, t5, t6, t7, t8) && right(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8) => self(t1, t2, t3, t4, t5, t6, t7, t8) || right(t1, t2, t3, t4, t5, t6, t7, t8);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8) => self(t1, t2, t3, t4, t5, t6, t7, t8) ^ right(t1, t2, t3, t4, t5, t6, t7, t8);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7, t8) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8))
                    return self(t1, t2, t3, t4, t5, t6, t7, t8);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8) => precededBy(t1, t2, t3, t4, t5, t6, t7, t8).Or(self(t1, t2, t3, t4, t5, t6, t7, t8));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8) => self(t1, t2, t3, t4, t5, t6, t7, t8).Or(followedBy(t1, t2, t3, t4, t5, t6, t7, t8));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7, t8);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7, t8);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9))
                    return self(t1, t2, t3, t4, t5, t6, t7, t8, t9);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => precededBy(t1, t2, t3, t4, t5, t6, t7, t8, t9).Or(self(t1, t2, t3, t4, t5, t6, t7, t8, t9));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9).Or(followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7, t8, t9);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7, t8, t9);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10))
                    return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => precededBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10).Or(self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10).Or(followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11))
                    return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => precededBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11).Or(self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11).Or(followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12))
                    return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => precededBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12).Or(self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12).Or(followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13))
                    return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => precededBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13).Or(self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13).Or(followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14))
                    return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => precededBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14).Or(self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14).Or(followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15))
                    return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => precededBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15).Or(self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15).Or(followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            };
        }

		
		public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> ToAction<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRet>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> And<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) && right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> Or<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");
            
            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) || right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> XOr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> right)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(right, "right");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) ^ right(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition, TResult defaultValue)
        {
            return MakeConditional(self, condition, (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => defaultValue);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> MakeConditional<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> condition, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                if (condition(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16))
                    return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);

                if (falseFunc != null)
                    return falseFunc(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                
                return default(TResult);
            };
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Maybe<TResult>> PrecededBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Maybe<TResult>> precededBy)
        {
            if (self == null || precededBy == null)
                return self ?? precededBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => precededBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16).Or(self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16));
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Maybe<TResult>> FollowedBy<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Maybe<TResult>> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Maybe<TResult>> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16).Or(followedBy(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16));
        }

		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> self)
        {
            return self.Synchronize((t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => true);
        }

        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Synchronize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> self, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, bool> needsSynchronizationPredicate)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(needsSynchronizationPredicate, "needsSynchronizationPredicate");

            var lockObject = new object();

            return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
            {
                if(needsSynchronizationPredicate(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16))
                {
                    lock (lockObject)
                    {
                        return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
                    }
                }

                return self(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            };
        }

		
		
		public static Func<T1, TResult> Memoize<T1, TResult>(this Func<T1, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1>, TResult>(x => self(x.Item1));

			return (t1) => dictionary[new Tuple<T1>(t1)];
		}
		
		public static Func<T1, T2, TResult> Memoize<T1, T2, TResult>(this Func<T1, T2, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2>, TResult>(x => self(x.Item1, x.Item2));

			return (t1, t2) => dictionary[new Tuple<T1, T2>(t1, t2)];
		}
		
		public static Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3>, TResult>(x => self(x.Item1, x.Item2, x.Item3));

			return (t1, t2, t3) => dictionary[new Tuple<T1, T2, T3>(t1, t2, t3)];
		}
		
		public static Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4));

			return (t1, t2, t3, t4) => dictionary[new Tuple<T1, T2, T3, T4>(t1, t2, t3, t4)];
		}
		
		public static Func<T1, T2, T3, T4, T5, TResult> Memoize<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5));

			return (t1, t2, t3, t4, t5) => dictionary[new Tuple<T1, T2, T3, T4, T5>(t1, t2, t3, t4, t5)];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, TResult> Memoize<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6));

			return (t1, t2, t3, t4, t5, t6) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6>(t1, t2, t3, t4, t5, t6)];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7));

			return (t1, t2, t3, t4, t5, t6, t7) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7>(t1, t2, t3, t4, t5, t6, t7)];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1));

			return (t1, t2, t3, t4, t5, t6, t7, t8) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8>(t8))];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9>>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2));

			return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9>(t8, t9))];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10>>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3));

			return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10>(t8, t9, t10))];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11>>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4));

			return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11>(t8, t9, t10, t11))];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12>>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5));

			return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12>(t8, t9, t10, t11, t12))];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13>>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5, x.Rest.Item6));

			return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12, T13>(t8, t9, t10, t11, t12, t13))];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14>>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5, x.Rest.Item6, x.Rest.Item7));

			return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12, T13, T14>(t8, t9, t10, t11, t12, t13, t14))];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15>>>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5, x.Rest.Item6, x.Rest.Item7, x.Rest.Rest.Item1));

			return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15>>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15>>(t8, t9, t10, t11, t12, t13, t14, new Tuple<T15>(t15)))];
		}
		
		public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> self)
		{
			Guard.NotNull(self, "self");
			var dictionary = new LazySelectionDictionary<Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16>>>, TResult>(x => self(x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Rest.Item1, x.Rest.Item2, x.Rest.Item3, x.Rest.Item4, x.Rest.Item5, x.Rest.Item6, x.Rest.Item7, x.Rest.Rest.Item1, x.Rest.Rest.Item2));

			return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) => dictionary[new Tuple<T1, T2, T3, T4, T5, T6, T7, Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16>>>(t1, t2, t3, t4, t5, t6, t7, new Tuple<T8, T9, T10, T11, T12, T13, T14, Tuple<T15, T16>>(t8, t9, t10, t11, t12, t13, t14, new Tuple<T15, T16>(t15, t16)))];
		}
			}
}
