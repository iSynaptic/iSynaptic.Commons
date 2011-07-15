using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons.Data
{
    public class AttributeExodataBindingSource : IExodataBindingSource
    {
        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            Guard.NotNull(request, "request");

            ICustomAttributeProvider provider = request.Member ?? typeof(TSubject);

            return provider.GetAttributesOfType<IExodataAttribute<TExodata>>()
                .Where(x => x.ProvidesExodataFor(request))
                .Select(x => ExodataBinding.Create<TExodata, TContext, TSubject>(this, y => true, r => x.Resolve(r).ToMaybe()));
        }
    }
}
