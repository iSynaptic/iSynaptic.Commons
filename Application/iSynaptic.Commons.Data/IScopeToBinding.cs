using System;

namespace iSynaptic.Commons.Data
{
    public interface IScopeToBinding<TMetadata> : IToBinding<TMetadata>
    {
        IToBinding<TMetadata> InScope(object scopeObject);
        IToBinding<TMetadata> InScope(Func<MetadataRequest<TMetadata>, object> scopeFactory);
    }
}