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
        private static readonly Lazy<Action<Exception>> _RethrowAction = null;

        static ExceptionExtensions()
        {
            _RethrowAction = new Lazy<Action<Exception>>(BuildRethrower);
        }

        public static void ThrowPreservingCallStack(this Exception self)
        {
            Guard.NotNull(self, "self");

            if(!string.IsNullOrWhiteSpace(self.StackTrace))
                _RethrowAction.Value(self);

            throw self;
        }

        private static Action<Exception> BuildRethrower()
        {
            Type exceptionType = typeof (Exception);
            var preserveStackTrace = exceptionType.GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

            var parameter = Expression.Parameter(exceptionType);
            
            var callExpression = Expression.Call(parameter, preserveStackTrace);
            var throwExpression = Expression.Throw(parameter);

            var expression = Expression.Block(callExpression, throwExpression);

            var lambda = Expression.Lambda<Action<Exception>>(expression, false, parameter);
            
            return lambda.Compile();
        }
    }
}
