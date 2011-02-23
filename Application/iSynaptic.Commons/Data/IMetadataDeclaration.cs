using System;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataDeclaration
    {
    }

    public interface IMetadataDeclaration<out TMetadata> : IMetadataDeclaration
    {
        TMetadata Resolve<TSubject>(Maybe<TSubject> subject, MemberInfo member);
    }
}