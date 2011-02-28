using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public static class ExodataHelper
    {
        public static MemberInfo ExtractMemberInfoForExodata(this Expression member)
        {
            Guard.NotNull(member, "member");

            var lambda = member as LambdaExpression;
            if (lambda != null)
            {
                var body = lambda.Body;

                if (body.NodeType == ExpressionType.Convert)
                    body = ((UnaryExpression)body).Operand;

                if (body is MemberExpression)
                {
                    var memberExpression = (MemberExpression)body;

                    if (memberExpression.Expression is ParameterExpression && memberExpression.Member is PropertyInfo ||
                        memberExpression.Member is FieldInfo)
                        return memberExpression.Member;
                }
            }

            throw new ArgumentException("You can only retreive member metatdata for properties and fields.", "member");
        }

    }
}
