using System;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IExodataRequest<out TExodata, out TContext, out TSubject>
    {
        IExodataDeclaration<TExodata> Declaration { get; }
        IMaybe<TContext> Context { get; }
        IMaybe<TSubject> Subject { get; }
        MemberInfo Member { get; }
    }
}