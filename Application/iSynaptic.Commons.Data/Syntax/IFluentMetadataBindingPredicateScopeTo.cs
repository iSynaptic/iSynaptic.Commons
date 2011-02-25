using System;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> : IFluentMetadataBindingScopeTo<TMetadata, TSubject>
    {
        IFluentMetadataBindingScopeTo<TMetadata, TSubject> When(Func<IMetadataRequest<TSubject>, bool> predicate);
    }
}