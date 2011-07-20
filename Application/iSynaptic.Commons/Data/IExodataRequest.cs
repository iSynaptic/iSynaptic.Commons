using System;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IExodataRequest<in TExodata, TContext, TSubject>
    {
        ISymbol Symbol { get; }

        Maybe<TContext> Context { get; }
        Maybe<TSubject> Subject { get; }

        MemberInfo Member { get; }
    }
}