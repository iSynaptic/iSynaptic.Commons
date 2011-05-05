using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons
{
    public sealed class SystemClock : Scope<SystemClock>
    {
        private static bool _PreventClockAlterations = false;

        private SystemClock()
            : this(ScopeBounds.Thread, ScopeNesting.Allowed)
        {
        }

        private SystemClock(ScopeBounds bounds, ScopeNesting nesting)
            : base(bounds, nesting)
        {
        }

        public static IDisposable Fixed(DateTime dateTime)
        {
            return Using(() => dateTime);
        }

        public static IDisposable Using(Func<DateTime> strategy)
        {
            if (_PreventClockAlterations)
                throw new InvalidOperationException("Clock alterations are not permitted.");

            return new SystemClock { UtcNowStrategy = Guard.NotNull(strategy, "strategy") };
        }

        public static void PreventClockAlterations()
        {
            _PreventClockAlterations = true;
        }

        public static Func<DateTime> DefaultDateTimeStrategy { get; set; }
        private Func<DateTime> UtcNowStrategy { get; set; }

        public static DateTime UtcNow
        {
            get
            {
                if (_PreventClockAlterations != true)
                {
                    var current = GetCurrentScope();

                    if (current != null && current.UtcNowStrategy != null)
                        return current.UtcNowStrategy();

                    if (DefaultDateTimeStrategy != null)
                        return DefaultDateTimeStrategy();
                }

                return DateTime.UtcNow;
            }
        }
    }
}
