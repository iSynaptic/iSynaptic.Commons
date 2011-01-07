using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataBinding<TMetadata> : IMetadataBinding<TMetadata>
    {
        public MetadataBinding(IMetadataDeclaration<TMetadata> declaration, TMetadata value)
            : this(r => r.Declaration == declaration, value)
        {
            Guard.NotNull(declaration, "declaration");
        }

        public MetadataBinding(Func<MetadataRequest<TMetadata>, bool> predicate, TMetadata value)
            : this(predicate, x => value)
        {
        }

        public MetadataBinding(Func<MetadataRequest<TMetadata>, bool> predicate, Func<MetadataRequest<TMetadata>, TMetadata> valueFactory)
            : this(predicate, valueFactory, null)
        {
        }

        public MetadataBinding(Func<MetadataRequest<TMetadata>, bool> predicate, Func<MetadataRequest<TMetadata>, TMetadata> valueFactory, Func<MetadataRequest<TMetadata>, object> scopeFactory)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");

            Predicate = predicate;
            ValueFactory = valueFactory;
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
    }
}
