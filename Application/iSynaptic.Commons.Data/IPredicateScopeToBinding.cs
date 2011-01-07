using System;

namespace iSynaptic.Commons.Data
{
    public interface IPredicateScopeToBinding<TMetadata> : IScopeToBinding<TMetadata>
    {
        IScopeToBinding<TMetadata> When(Func<MetadataRequest<TMetadata>, bool> predicate);
    }
}