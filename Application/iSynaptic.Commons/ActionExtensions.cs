using System;
using System.Collections.Generic;
using System.Threading;

namespace iSynaptic.Commons
{
    public static class ActionExtensions
    {
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
    }
}
