using System;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IScopeToBinding<TMetadata, TSubject> : IToBinding<TMetadata, TSubject>
    {
        IToBinding<TMetadata, TSubject> InScope(object scopeObject);
        IToBinding<TMetadata, TSubject> InScope(Func<MetadataRequest<TMetadata, TSubject>, object> scopeFactory);
    }
}