using System;

namespace iSynaptic.Commons.Data
{
    internal class AttributeMetadataBinding<TMetadata> : IMetadataBinding<TMetadata>
    {
        private readonly IMetadataAttribute<TMetadata> _Attribute;

        public AttributeMetadataBinding(IMetadataAttribute<TMetadata> attribute)
        {
            Guard.NotNull(attribute, "attribute");
            _Attribute = attribute;
        }

        public bool Matches(MetadataRequest<TMetadata> request)
        {
            return _Attribute.ProvidesMetadataFor(request);
        }

        public Func<MetadataRequest<TMetadata>, object> ScopeFactory
        {
            get { return null; }
        }

        public TMetadata Resolve(MetadataRequest<TMetadata> request)
        {
            return _Attribute.Resolve(request);
        }
    }
}