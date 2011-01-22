using System;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataRequest<in TMetadata, out TSubject>
    {
        IMetadataDeclaration<TMetadata> Declaration { get; }
        Func<TSubject> Subject { get; }
        MemberInfo Member { get; }
    }
}