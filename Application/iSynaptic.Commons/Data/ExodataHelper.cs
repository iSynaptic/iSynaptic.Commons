using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Linq.Expressions;

namespace iSynaptic.Commons.Data
{
    public static class ExodataHelper
    {
        public static MemberInfo ExtractMemberInfoForExodata<TSubject>(this Expression member)
        {
            Guard.NotNull(member, "member");

            return member.ExtractMemberInfoFromMemberExpression()
                .Where(x => x.DeclaringType.IsAssignableFrom(typeof(TSubject)))
                .Where(x => x is PropertyInfo || x is FieldInfo)
                .ThrowOnNoValue(() => new ArgumentException("You can only retrieve member exodata for properties and fields.", "member"))
                .Value;
        }
    }
}
