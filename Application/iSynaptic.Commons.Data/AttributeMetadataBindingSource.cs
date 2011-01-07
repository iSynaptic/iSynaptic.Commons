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
            Guard.NotNull(request, "request");

            if(request.Subject == null && request.Member == null)
                yield break;

            ICustomAttributeProvider provider = request.Member;

            if (provider == null)
                provider = request.Subject as Type;

            if (provider == null)
                provider = request.Subject.GetType();

            var attributes = provider.GetAttributesOfType<IMetadataAttribute<TMetadata>>()
                .Where(x => x.ProvidesMetadataFor(request));

            foreach (var attribute in attributes)
                yield return new AttributeMetadataBinding<TMetadata>(attribute);
        }
    }
}
