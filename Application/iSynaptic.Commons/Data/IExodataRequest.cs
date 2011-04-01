using System;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IExodataRequest<out TContext, out TSubject>
    {
        IExodataDeclaration Declaration { get; }
        IMaybe<TContext> Context { get; }
        IMaybe<TSubject> Subject { get; }
        MemberInfo Member { get; }
    }
}