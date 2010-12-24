using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons.Data
{
    public class AttributeMetadataBindingSource : IMetadataBindingSource
    {
        public IEnumerable<IMetadataBinding<TMetadata>> GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            if(request.Subject == null && request.Member == null)
                yield break;

            ICustomAttributeProvider provider = request.Member;

            if (provider == null)
                provider = request.Subject as Type;

            if (provider == null)
                provider = request.Subject.GetType();

            var attributes = provider.GetAttributesOfType<IMetadataAttribute>()
                .Where(x => x.ProvidesMetadataFor(request));

            foreach (var attribute in attributes)
                yield return new AttributeMetadataBinding<TMetadata>(attribute);
        }

        private class AttributeMetadataBinding<TMetadata> : IMetadataBinding<TMetadata>
        {
            private readonly IMetadataAttribute _Attribute;

            public AttributeMetadataBinding(IMetadataAttribute attribute)
            {
                _Attribute = attribute;
            }

            public bool Matches(MetadataRequest<TMetadata> request)
            {
                return _Attribute.ProvidesMetadataFor(request);
            }

            public Func<MetadataRequest<TMetadata>, object> ScopeFactory
            {
                get { throw new NotImplementedException(); }
            }

            public TMetadata Resolve(MetadataRequest<TMetadata> request)
            {
                return _Attribute.Resolve(request);
            }
        }
    }
}
