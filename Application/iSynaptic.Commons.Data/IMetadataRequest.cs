using System;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataRequest<in TMetadata, out TSubject>
    {
        IMetadataDeclaration Declaration { get; }
        IMaybe<TSubject> Subject { get; }
        MemberInfo Member { get; }
    }
}