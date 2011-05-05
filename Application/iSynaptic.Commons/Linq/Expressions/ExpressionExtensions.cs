using System;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T1, TCovariantResult>> ToCovariant<T1, TResult, TCovariantResult>(this Expression<Func<T1, TResult>> self)
            where TResult : TCovariantResult
        {
            return Expression.Lambda<Func<T1, TCovariantResult>>(self.Body, self.Parameters);
        }

        public static Maybe<MemberInfo> ExtractMemberInfoFromMemberExpression(this Expression expression)
        {
            Guard.NotNull(expression, "expression");

            return Maybe.Value(expression)
                .Select(x =>
                        {
                            var memberInfo = Maybe<MemberInfo>.NoValue;
                            new ExtractMemberInfoFromMemberExpressionVisitor(y => memberInfo = y).Visit(x);
                            return memberInfo;
                        });
        }

        private class ExtractMemberInfoFromMemberExpressionVisitor : ExpressionVisitor
        {
            private readonly Action<MemberInfo> _Action;

            public ExtractMemberInfoFromMemberExpressionVisitor(Action<MemberInfo> action)
            {
                _Action = action;
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                _Action(node.Member);
                return node;
            }
        }
    }
}
