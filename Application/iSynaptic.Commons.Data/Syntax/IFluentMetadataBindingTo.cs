using System;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentMetadataBindingTo<TMetadata, TSubject> : IFluentInterface
    {
        void To(TMetadata value);
        void To(Func<IMetadataRequest<TSubject>, TMetadata> valueFactory);
    }
}