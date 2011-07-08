using System;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IExodataRequest<in TExodata, out TContext, out TSubject>
    {
        ISymbol<TExodata> Symbol { get; }
        IMaybe<TContext> Context { get; }
        IMaybe<TSubject> Subject { get; }
        MemberInfo Member { get; }
    }
}