// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
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
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T1, TCovariantResult>> ToCovariant<T1, TResult, TCovariantResult>(this Expression<Func<T1, TResult>> @this)
            where TResult : TCovariantResult
        {
            return Expression.Lambda<Func<T1, TCovariantResult>>(@this.Body, @this.Parameters);
        }

        public static Maybe<MemberInfo> ExtractMemberInfoFromMemberExpression(this Expression expression)
        {
            Guard.NotNull(expression, "expression");

            return expression
                .ToMaybe()
                .SelectMaybe(x =>
                        {
                            var memberInfo = Maybe<MemberInfo>.NoValue;
                            new ExtractMemberInfoFromMemberExpressionVisitor(y => memberInfo = y.ToMaybe()).Visit(x);
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
