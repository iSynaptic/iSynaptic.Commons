using System;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IToBinding<TMetadata, TSubject> : IFluentInterface
    {
        void To(TMetadata value);
        void To(Func<MetadataRequest<TMetadata, TSubject>, TMetadata> valueFactory);
    }
}