using System;
using System.Collections.Generic;
using System.Threading;

namespace iSynaptic.Commons
{
    public static partial class ActionExtensions
    {
        public static Action Curry<T1>(this Action<T1> @this, T1 t1)
        {
            Guard.NotNull(@this, "@this");
            return () => @this(t1);
        }

        public static IDisposable ToDisposable(this Action @this)
        {
            Guard.NotNull(@this, "@this");
            return ToDisposable(disposing => @this());
        }

        public static IDisposable ToDisposable(this Action<bool> @this)
        {
            return new ActionDisposer(Guard.NotNull(@this, "@this"));
        }

        private sealed class ActionDisposer : IDisposable
        {
            private readonly Action<bool> _Action = null;

            public ActionDisposer(Action<bool> action)
            {
                Guard.NotNull(action, "action");
                _Action = action;
            }

            ~ActionDisposer()
            {
                Dispose(false);
            }

            public void Dispose()
            {
                try
                {
                    Dispose(true);
                }
                finally
                {
                    GC.SuppressFinalize(this);
                }
            }

            private void Dispose(bool disposing)
            {
                _Action(disposing);
            }
        }

        public static Action MakeIdempotent(this Action @this)
        {
            Guard.NotNull(@this, "@this");

            int beenExecuted = 0;

            return () =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if (previousValue == 0)
                {
                    @this();
                    @this = null;
                }
            };
        }

        public static Action CatchExceptions(this Action @this)
        {
            Guard.NotNull(@this, "@this");
            return CatchExceptions(@this, null);
        }

        public static Action CatchExceptions(this Action @this, ICollection<Exception> exceptions)
        {
            Guard.NotNull(@this, "@this");
            return () =>
            {
                try
                {
                    @this();
                }
                catch (Exception ex)
                {
                    if (exceptions != null)
                        exceptions.Add(ex);
                }
            };
        }

        public static Action FollowedBy(this Action @this, Action followedBy)
        {
            if (@this == null || followedBy == null)
                return @this ?? followedBy;

            return () =>
            {
                @this();
                followedBy();
            };
        }
    }
}
