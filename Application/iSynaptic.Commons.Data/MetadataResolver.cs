using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public abstract class MetadataResolver : IMetadataResolver
    {
        private HashSet<IMetadataBindingSource> _BindingSources = new HashSet<IMetadataBindingSource>();

        public TMetadata Resolve<TMetadata>(MetadataDeclaration<TMetadata> declaration, object subject, MemberInfo member)
        {
            var request = new MetadataRequest<TMetadata>(declaration, subject, member);

            var candidateBindings = _BindingSources
                .SelectMany(x => x.GetBindingsFor(request))
                .Where(x => x.Matches(request));
                
            var selectedBinding = SelectBinding(request, candidateBindings);

            if(selectedBinding == null)
                return declaration.Default;

            var results = selectedBinding.Resolve(request);
            declaration.ValidateValue(results);

            return results;
        }

        protected abstract IMetadataBinding<TMetadata> SelectBinding<TMetadata>(MetadataRequest<TMetadata> request, IEnumerable<IMetadataBinding<TMetadata>> candidates);

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
