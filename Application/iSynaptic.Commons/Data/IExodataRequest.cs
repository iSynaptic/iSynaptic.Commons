using System;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IExodataRequest<out TSubject>
    {
        IExodataDeclaration Declaration { get; }
        IMaybe<TSubject> Subject { get; }
        MemberInfo Member { get; }
    }
}