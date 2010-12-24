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
            var request = new MetadataRequest<T>(declaration, subject, member);

            return _BindingSources
                .SelectMany(x => x.GetBindingsFor<T>(request))
                .Where(x => x.Matches(request))
                .Take(1)
                .Select(x => x.Resolve(request))
                .DefaultIfEmpty(declaration.Default)
                .First();
        }

        public void AddMetadataBindingSource<T>() where T : class, IMetadataBindingSource, new()
        {
            AddMetadataBindingSource(new T());
        }

        public void AddMetadataBindingSource(IMetadataBindingSource source)
        {
            if(source == null)
                throw new ArgumentNullException("source");

            _BindingSources.Add(source);
        }
    }
}
