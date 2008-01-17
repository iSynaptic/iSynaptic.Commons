using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Extensions
{
    public static class ActionExtensions
    {
        #region Curry Methods

        public static Action Curry<T1>(this Action<T1> f, T1 arg1)
        {
            return () => f(arg1);
        }

        public static Action Curry<T1, T2>(this Action<T1, T2> f, T1 arg1, T2 arg2)
        {
            return () => f(arg1, arg2);
        }

        public static Action<T2> Curry<T1, T2>(this Action<T1, T2> f, T1 arg1)
        {
            return t2 => f(arg1, t2);
        }

        public static Action Curry<T1, T2, T3>(this Action<T1, T2, T3> f, T1 arg1, T2 arg2, T3 arg3)
        {
            return () => f(arg1, arg2, arg3);
        }

        public static Action<T3> Curry<T1, T2, T3>(this Action<T1, T2, T3> f, T1 arg1, T2 arg2)
        {
            return t3 => f(arg1, arg2, t3);
        }

        public static Action<T2, T3> Curry<T1, T2, T3>(this Action<T1, T2, T3> f, T1 arg1)
        {
            return (t2, t3) => f(arg1, t2, t3);
        }

        public static Action Curry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> f, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return () => f(arg1, arg2, arg3, arg4);
        }

        public static Action<T4> Curry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> f, T1 arg1, T2 arg2, T3 arg3)
        {
            return t4 => f(arg1, arg2, arg3, t4);
        }

        public static Action<T3, T4> Curry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> f, T1 arg1, T2 arg2)
        {
            return (t3, t4) => f(arg1, arg2, t3, t4);
        }

        public static Action<T2, T3, T4> Curry<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> f, T1 arg1)
        {
            return (t2, t3, t4) => f(arg1, t2, t3, t4);
        }

        #endregion

        #region MakeConditional Methods

        public static Action<T> MakeConditional<T>(this Action<T> self, Predicate<T> condition)
        {
            return MakeConditional(self, null, condition);
        }

        public static Action<T> MakeConditional<T>(this Action<T> self, Action<T> falseAction, Predicate<T> condition)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            if (condition == null)
                throw new ArgumentNullException("condition");

            return item =>
            {
                if (condition(item))
                    self(item);
                else if (falseAction != null)
                    falseAction(item);
            };
        }

        #endregion

        #region ToDisposable

        public static IDisposable ToDisposable(this Action self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            return new DisposableAction(self);
        }

        private class DisposableAction : IDisposable
        {
            private Action _Action = null;
            public DisposableAction(Action action)
            {
                _Action = action;
            }

            public void Dispose()
            {
                _Action();
            }
        }

        #endregion
    }
}
