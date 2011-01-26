using System;
using System.Collections.Generic;
using System.Threading;

namespace iSynaptic.Commons
{
    public static partial class ActionExtensions
    {
        public static IDisposable ToDisposable(this Action self)
        {
            Guard.NotNull(self, "self");
            return new ActionDisposer(self);
        }

        public static IDisposable ToDisposable(this Action<bool> self)
        {
            Guard.NotNull(self, "self");
            return new ActionDisposer(self);
        }

        private sealed class ActionDisposer : IDisposable
        {
            private readonly Action<bool> _Action = null;
            public ActionDisposer(Action action)
            {
                Guard.NotNull(action, "action");
                _Action = disposing => action();
            }

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
                Dispose(true);
                GC.SuppressFinalize(this);
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
                int previousValue = Interlocked.Increment(ref beenExecuted);

                if (previousValue == 0)
                    self();
                else
                    beenExecuted = 1;
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
