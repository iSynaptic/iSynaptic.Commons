using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace iSynaptic.Commons
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T1, TCovariantResult>> ToCovariant<T1, TResult, TCovariantResult>(this Expression<Func<T1, TResult>> self)
            where TResult : TCovariantResult
        {
            return Expression.Lambda<Func<T1, TCovariantResult>>(self.Body, self.Parameters);
        }
    }
}
