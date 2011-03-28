using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons
{
    public static class ExceptionExtensions
    {
        private static readonly Action<Exception> _PreserveStackTrace =
                (Action<Exception>)Delegate.CreateDelegate(
                    typeof(Action<Exception>),
                    typeof(Exception).GetMethod(
                        "InternalPreserveStackTrace",
                        BindingFlags.Instance | BindingFlags.NonPublic));

        public static void ThrowPreservingCallStack(this Exception self)
        {
            Guard.NotNull(self, "self");

            if (!string.IsNullOrWhiteSpace(self.StackTrace))
                _PreserveStackTrace(self);

            throw self;
        }
    }
}
