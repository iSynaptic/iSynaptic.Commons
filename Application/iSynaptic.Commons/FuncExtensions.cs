using System;

namespace iSynaptic.Commons
{
    public static class FuncExtensions
    {
        #region ToAction Methods

        public static Action ToAction<TRet>(this Func<TRet> self)
        {
            Guard.NotNull(self, "self");
            return () => self();
        }

        public static Action<T> ToAction<T, TRet>(this Func<T, TRet> self)
        {
            Guard.NotNull(self, "self");
            return t => self(t);
        }

        public static Action<T1, T2> ToAction<T1, T2, TRet>(this Func<T1, T2, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2) => self(t1, t2);
        }

        public static Action<T1, T2, T3> ToAction<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3) => self(t1, t2, t3);
        }

        public static Action<T1, T2, T3, T4> ToAction<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> self)
        {
            Guard.NotNull(self, "self");
            return (t1, t2, t3, t4) => self(t1, t2, t3, t4);
        }

        #endregion

        #region Curry Methods

        public static Func<TRet> Curry<T1, TRet>(this Func<T1, TRet> f, T1 arg1)
        {
            Guard.NotNull(f, "f");
            return () => f(arg1);
        }

        public static Func<TRet> Curry<T1, T2, TRet>(this Func<T1, T2, TRet> f, T1 arg1, T2 arg2)
        {
            Guard.NotNull(f, "f");
            return () => f(arg1, arg2);
        }

        public static Func<T2, TRet> Curry<T1, T2, TRet>(this Func<T1, T2, TRet> f, T1 arg1)
        {
            Guard.NotNull(f, "f");
            return t2 => f(arg1, t2);
        }

        public static Func<TRet> Curry<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> f, T1 arg1, T2 arg2, T3 arg3)
        {
            Guard.NotNull(f, "f");
            return () => f(arg1, arg2, arg3);
        }

        public static Func<T3, TRet> Curry<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> f, T1 arg1, T2 arg2)
        {
            Guard.NotNull(f, "f");
            return t3 => f(arg1, arg2, t3);
        }

        public static Func<T2, T3, TRet> Curry<T1, T2, T3, TRet>(this Func<T1, T2, T3, TRet> f, T1 arg1)
        {
            Guard.NotNull(f, "f");
            return (t2, t3) => f(arg1, t2, t3);
        }

        public static Func<TRet> Curry<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> f, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Guard.NotNull(f, "f");
            return () => f(arg1, arg2, arg3, arg4);
        }

        public static Func<T4, TRet> Curry<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> f, T1 arg1, T2 arg2, T3 arg3)
        {
            Guard.NotNull(f, "f");
            return t4 => f(arg1, arg2, arg3, t4);
        }

        public static Func<T3, T4, TRet> Curry<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> f, T1 arg1, T2 arg2)
        {
            Guard.NotNull(f, "f");
            return (t3, t4) => f(arg1, arg2, t3, t4);
        }

        public static Func<T2, T3, T4, TRet> Curry<T1, T2, T3, T4, TRet>(this Func<T1, T2, T3, T4, TRet> f, T1 arg1)
        {
            Guard.NotNull(f, "f");
            return (t2, t3, t4) => f(arg1, t2, t3, t4);
        }

        #endregion

        #region MakeConditional Methods

        public static Func<T, TRet> MakeConditional<T, TRet>(this Func<T, TRet> self, Predicate<T> condition)
        {
            return MakeConditional(self, condition, null);
        }

        public static Func<T, TRet> MakeConditional<T, TRet>(this Func<T, TRet> self, Predicate<T> condition, TRet defaultValue)
        {
            return MakeConditional(self, condition, item => defaultValue);
        }

        public static Func<T, TRet> MakeConditional<T, TRet>(this Func<T, TRet> self, Predicate<T> condition, Func<T, TRet> falseFunc)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

            return item =>
            {
                if (condition(item))
                    return self(item);

                if (falseFunc != null)
                    return falseFunc(item);
                
                return default(TRet);
            };
        } 
        #endregion

        #region Logic Composition Methods

        public static Func<T, bool> And<T>(this Func<T, bool> self, Func<T, bool> right)
        {
            if(self == null)
                throw new ArgumentNullException("self");

            if(right == null)
                throw new ArgumentNullException("right");

            return input => self(input) && right(input);
        }

        public static Func<T, bool> Or<T>(this Func<T, bool> self, Func<T, bool> right)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            if (right == null)
                throw new ArgumentNullException("right");
            
            return input => self(input) || right(input);
        }

        public static Func<T, bool> XOr<T>(this Func<T, bool> self, Func<T, bool> right)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            if (right == null)
                throw new ArgumentNullException("right");
            
            return input => self(input) ^ right(input);
        }

        #endregion
    }
}
