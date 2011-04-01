using System;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IExodataDeclaration
    {
    }

    public interface IExodataDeclaration<out TExodata> : IExodataDeclaration
    {
        TExodata Resolve<TContext, TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member);
    }
}