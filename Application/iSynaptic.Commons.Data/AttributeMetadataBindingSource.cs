using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons.Data
{
    public class AttributeMetadataBindingSource : IMetadataBindingSource
    {
        public IEnumerable<IMetadataBinding<TMetadata, TSubject>> GetBindingsFor<TMetadata, TSubject>(IMetadataRequest<TSubject> request)
        {
            Guard.NotNull(request, "request");

            ICustomAttributeProvider provider = request.Member ?? typeof(TSubject);

            return provider.GetAttributesOfType<IMetadataAttribute<TMetadata>>()
                .Where(x => x.ProvidesMetadataFor(request))
                .Select(x => new MetadataBinding<TMetadata, TSubject>(x.ProvidesMetadataFor, x.Resolve, this));
        }
    }
}
