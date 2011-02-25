using System;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentMetadataBindingScopeTo<TMetadata, TSubject> : IFluentMetadataBindingTo<TMetadata, TSubject>
    {
        IFluentMetadataBindingTo<TMetadata, TSubject> InScope(object scopeObject);
        IFluentMetadataBindingTo<TMetadata, TSubject> InScope(Func<IMetadataRequest<TSubject>, object> scopeFactory);
    }
}