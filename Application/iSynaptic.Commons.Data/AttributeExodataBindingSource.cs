using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons.Data
{
    public class AttributeExodataBindingSource : IExodataBindingSource
    {
        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TSubject>(IExodataRequest<TSubject> request)
        {
            Guard.NotNull(request, "request");

            ICustomAttributeProvider provider = request.Member ?? typeof(TSubject);

            return provider.GetAttributesOfType<IExodataAttribute<TExodata>>()
                .Where(x => x.ProvidesExodataFor(request))
                .Select(x => ExodataBinding.Create<TExodata, TSubject>(this, x.ProvidesExodataFor, x.Resolve));
        }
    }
}
