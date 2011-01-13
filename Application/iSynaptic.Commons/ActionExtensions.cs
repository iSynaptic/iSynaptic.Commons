using System;
using System.Collections.Generic;

namespace iSynaptic.Commons
{
    public static class ActionExtensions
    {
        #region MakeConditional

        public static Action<T> MakeConditional<T>(this Action<T> self, Predicate<T> condition)
        {
            return MakeConditional(self, null, condition);
        }

        public static Action<T> MakeConditional<T>(this Action<T> self, Action<T> falseAction, Predicate<T> condition)
        {
            Guard.NotNull(self, "self");
            Guard.NotNull(condition, "condition");

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
            Guard.NotNull(self, "self");
            return new DisposableAction(self);
        }

        private class DisposableAction : IDisposable
        {
            private readonly Action _Action = null;
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

        #region CatchExceptions

        public static Action<T> CatchExceptions<T>(this Action<T> self)
        {
            return self.CatchExceptions(null);
        }

        public static Action<T> CatchExceptions<T>(this Action<T> self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");

            return x =>
            {
                Action innerAction = () => self(x);
                innerAction = innerAction.CatchExceptions(exceptions);

                innerAction();
            };
        }

        public static Action CatchExceptions(this Action self)
        {
            return CatchExceptions(self, null);
        }

        public static Action CatchExceptions(this Action self, ICollection<Exception> exceptions)
        {
            Guard.NotNull(self, "self");
            return () =>
            {
                try
                {
                    self();
                }
                catch (Exception ex)
                {
                    if (exceptions != null)
                        exceptions.Add(ex);
                }
            };
        }

        #endregion

        public static Action FollowedBy(this Action self, Action followedBy)
        {
            if(self == null || followedBy == null)
                return self ?? followedBy;

            return () =>
            {
                self();
                followedBy();
            };
        }

        public static Action<T> FollowedBy<T>(this Action<T> self, Action<T> followedBy)
        {
            if (self == null || followedBy == null)
                return self ?? followedBy;

            return x =>
            {
                self(x);
                followedBy(x);
            };
        }
    }
}
