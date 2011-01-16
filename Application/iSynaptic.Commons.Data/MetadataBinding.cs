using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataBinding<TMetadata> : IMetadataBinding<TMetadata>
    {
        public MetadataBinding(MetadataDeclaration<TMetadata> declaration, TMetadata value, IMetadataBindingSource source)
            : this(r => r.Declaration == declaration, value, source)
        {
            Guard.NotNull(declaration, "declaration");
        }

        public MetadataBinding(Func<MetadataRequest<TMetadata>, bool> predicate, TMetadata value, IMetadataBindingSource source)
            : this(predicate, x => value, source)
        {
        }

        public MetadataBinding(Func<MetadataRequest<TMetadata>, bool> predicate, Func<MetadataRequest<TMetadata>, TMetadata> valueFactory, IMetadataBindingSource source)
            : this(predicate, valueFactory, source, null)
        {
        }

        public MetadataBinding(Func<MetadataRequest<TMetadata>, bool> predicate, Func<MetadataRequest<TMetadata>, TMetadata> valueFactory, IMetadataBindingSource source, Func<MetadataRequest<TMetadata>, object> scopeFactory)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            Predicate = predicate;
            ValueFactory = valueFactory;
            Source = source;
            ScopeFactory = scopeFactory;
        }

        public bool Matches(MetadataRequest<TMetadata> request)
        {
            return Predicate(request);
        }

        public TMetadata Resolve(MetadataRequest<TMetadata> request)
        {
            return ValueFactory(request);
        }

        public Func<MetadataRequest<TMetadata>, bool> Predicate { get; private set; }
        public Func<MetadataRequest<TMetadata>, TMetadata> ValueFactory { get; private set; }
        public Func<MetadataRequest<TMetadata>, object> ScopeFactory { get; private set; }

        public IMetadataBindingSource Source { get; private set; }
    }
}
