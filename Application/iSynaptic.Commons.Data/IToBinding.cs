using System;

namespace iSynaptic.Commons.Data
{
    public interface IToBinding<TMetadata> : IFluentInterface
    {
        void To(TMetadata value);
        void To(Func<MetadataRequest<TMetadata>, TMetadata> valueFactory);
    }
}