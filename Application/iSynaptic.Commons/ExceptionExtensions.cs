using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons
{
    public static class ExceptionExtensions
    {
        private static readonly Action<Exception, Exception> _SetInnerException = null;

        static ExceptionExtensions()
        {
            var target = Expression.Parameter(typeof (Exception));
            var source = Expression.Parameter(typeof (Exception));

            var assignment = Expression.Assign(Expression.Field(target, "_innerException"), source);

            var lambda = Expression.Lambda<Action<Exception, Exception>>(assignment, target, source);
            _SetInnerException = lambda.Compile();
        }

        public static void ThrowAsInnerExceptionIfNeeded(this Exception self)
        {
            Guard.NotNull(self, "self");

            var newException = Cloneable<Exception>.ShallowClone(self);

            if (string.IsNullOrWhiteSpace(newException.StackTrace) != true)
                _SetInnerException(newException, self);

            throw newException;
        }
    }
}
