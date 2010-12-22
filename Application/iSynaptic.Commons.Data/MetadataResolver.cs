using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataResolver : IMetadataResolver
    {
        private HashSet<IMetadataBindingSource> _BindingSources = new HashSet<IMetadataBindingSource>();

        public T Resolve<T>(MetadataDeclaration<T> declaration, object subject, MemberInfo member)
        {
            var request = new MetadataRequest(declaration, subject, member);

            return _BindingSources
                .SelectMany(x => x.GetBindingsFor(request))
                .Where(x => x.Matches(request))
                .Take(1)
                .Select(x => x.Resolve<T>(request))
                .DefaultIfEmpty(declaration.Default)
                .First();
        }

        public void AddMetadataBindingSource(IMetadataBindingSource source)
        {
            if(source == null)
                throw new ArgumentNullException("source");

            _BindingSources.Add(source);
        }
    }
}
