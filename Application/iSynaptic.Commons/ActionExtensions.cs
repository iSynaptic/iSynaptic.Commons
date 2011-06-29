using System;
using System.Collections.Generic;
using System.Threading;

namespace iSynaptic.Commons
{
    public static partial class ActionExtensions
    {
        public static Action Curry<T1>(this Action<T1> self, T1 t1)
        {
            Guard.NotNull(self, "self");
            return () => self(t1);
        }

        public static IDisposable ToDisposable(this Action self)
        {
            Guard.NotNull(self, "self");
            return ToDisposable(disposing => self());
        }

        public static IDisposable ToDisposable(this Action<bool> self)
        {
            return new ActionDisposer(Guard.NotNull(self, "self"));
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

        public static Action MakeIdempotent(this Action self)
        {
            Guard.NotNull(self, "self");

            int beenExecuted = 0;

            return () =>
            {
                int previousValue = Interlocked.CompareExchange(ref beenExecuted, 1, 0);

                if (previousValue == 0)
                {
                    self();
                    self = null;
                }
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
    }
}
