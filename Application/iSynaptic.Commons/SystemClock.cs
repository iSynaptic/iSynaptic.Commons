using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.AOP;

namespace iSynaptic.Commons
{
    public class SystemClock : Scope<SystemClock>
    {
        private static Func<DateTime> _DefaultDateTimeStrategy = null;

        protected SystemClock()
            : this(ScopeBounds.AppDomain, ScopeNesting.Allowed)
        {
        }

        protected SystemClock(ScopeBounds bounds, ScopeNesting nesting)
            : base(bounds, nesting)
        {
        }

        public static IDisposable Fixed(DateTime dateTime)
        {
            return new SystemClock { DateTimeStrategy = () => dateTime };
        }

        public static IDisposable Using(Func<DateTime> strategy)
        {
            return new SystemClock { DateTimeStrategy = Guard.NotNull(strategy, "strategy") };
        }

        public static Func<DateTime> DefaultDateTimeStrategy
        {
            get { return _DefaultDateTimeStrategy ?? (() => DateTime.UtcNow); }
            set { _DefaultDateTimeStrategy = value; }
        }

        protected Func<DateTime> DateTimeStrategy { get; set; }

        public static DateTime Now
        {
            get
            {
                var current = GetCurrentScope();

                if (current != null && current.DateTimeStrategy != null)
                    return current.DateTimeStrategy();

                return DefaultDateTimeStrategy();
            }
        }
    }
}
