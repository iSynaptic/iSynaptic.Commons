using System;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IToBinding<TMetadata, TSubject> : IFluentInterface
    {
        void To(TMetadata value);
        void To(Func<IMetadataRequest<TMetadata, TSubject>, TMetadata> valueFactory);
    }
}