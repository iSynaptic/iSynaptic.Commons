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
            return new DisposableAction(self);
        }

        private class DisposableAction : IDisposable
        {
            private readonly Action _Action = null;
            public DisposableAction(Action action)
            {
                Guard.NotNull(action, "action");
                _Action = action;
            }

            public void Dispose()
            {
                _Action();
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
