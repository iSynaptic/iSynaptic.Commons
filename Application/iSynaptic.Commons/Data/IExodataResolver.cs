using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IExodataResolver
    {
        Maybe<TExodata> TryResolve<TExodata, TContext, TSubject>(ISymbol symbol, Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo memberInfo);
    }
}
