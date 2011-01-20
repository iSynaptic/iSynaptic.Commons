using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons.Data
{
    public class AttributeMetadataBindingSource : IMetadataBindingSource
    {
        public IEnumerable<IMetadataBinding<TMetadata>> GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            Guard.NotNull(request, "request");

            if (request.Subject == null && request.Member == null)
                return Enumerable.Empty<IMetadataBinding<TMetadata>>();

            ICustomAttributeProvider provider = request.Member;

            if (provider == null)
                provider = request.Subject as Type;

            if (provider == null)
                provider = request.Subject.GetType();

            return provider.GetAttributesOfType<IMetadataAttribute<TMetadata>>()
                .Where(x => x.ProvidesMetadataFor(request))
                .Select(x => new MetadataBinding<TMetadata>(x.ProvidesMetadataFor, x.Resolve, this));
        }
    }
}
