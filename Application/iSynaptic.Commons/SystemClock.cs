// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
        private static Func<DateTime> _defaultDateTimeStrategy;

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
            EnsureAlterationsAreAllowed();
            return new SystemClock { UtcNowStrategy = Guard.NotNull(strategy, "strategy") };
        }

        public static void PreventClockAlterations()
        {
            _PreventClockAlterations = true;
        }

        private static void EnsureAlterationsAreAllowed()
        {
            if (_PreventClockAlterations)
                throw new InvalidOperationException("Clock alterations are not permitted.");
        }

        public static Func<DateTime> DefaultDateTimeStrategy
        {
            get { return _defaultDateTimeStrategy; }
            set
            {
                EnsureAlterationsAreAllowed();
                _defaultDateTimeStrategy = value;
            }
        }

        private Func<DateTime> UtcNowStrategy { get; set; }

        public static DateTime UtcNow
        {
            get
            {
                DateTime value = DateTime.UtcNow;

                if (_PreventClockAlterations != true)
                {
                    var current = GetCurrentScope();

                    if (current != null && current.UtcNowStrategy != null)
                        value = current.UtcNowStrategy();
                    else if (DefaultDateTimeStrategy != null)
                        value = DefaultDateTimeStrategy();
                }

                if(value.Kind != DateTimeKind.Utc)
                    throw new InvalidOperationException("DateTime returned by strategy does not return a value in UTC.");

                return value;
            }
        }
    }
}
