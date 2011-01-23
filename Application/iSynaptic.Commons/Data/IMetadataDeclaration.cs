using System;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataDeclaration
    {
    }

    public interface IMetadataDeclaration<out TMetadata> : IMetadataDeclaration
    {
        TMetadata Resolve<TSubject>(IMetadataResolver resolver, Maybe<TSubject> subject, MemberInfo member);
        TMetadata Default { get; }
    }
}