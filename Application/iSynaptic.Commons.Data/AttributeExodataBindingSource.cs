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
                .Select(x => ExodataBinding.Create<TExodata, object, TSubject>(null, this, Maybe<ISymbol>.NoValue, Maybe<object>.NoValue, Maybe<TSubject>.NoValue, request.Member != null ? new[]{request.Member} : null, null, r => x.TryResolve(r).AsMaybe()));
        }
    }
}
