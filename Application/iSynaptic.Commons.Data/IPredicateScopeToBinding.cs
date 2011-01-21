using System;

namespace iSynaptic.Commons.Data
{
    public interface IPredicateScopeToBinding<TMetadata, TSubject> : IScopeToBinding<TMetadata, TSubject>
    {
        IScopeToBinding<TMetadata, TSubject> When(Func<MetadataRequest<TMetadata, TSubject>, bool> predicate);
    }
}