using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataBinding<TMetadata> : IMetadataBinding<TMetadata>
    {
        public MetadataBinding(Func<MetadataRequest<TMetadata>, bool> predicate, Func<MetadataRequest<TMetadata>, TMetadata> valueFactory, IMetadataBindingSource source)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            Predicate = predicate;
            ValueFactory = valueFactory;
            Source = source;
        }

        public bool Matches(MetadataRequest<TMetadata> request)
        {
            return Predicate(request);
        }

        public TMetadata Resolve(MetadataRequest<TMetadata> request)
        {
            return ValueFactory(request);
        }

        public object Subject { get; set; }
        public MemberInfo Member { get; set; }
        public Func<MetadataRequest<TMetadata>, object> ScopeFactory { get; set; }

        public Func<MetadataRequest<TMetadata>, bool> Predicate { get; private set; }
        public Func<MetadataRequest<TMetadata>, TMetadata> ValueFactory { get; private set; }

        public IMetadataBindingSource Source { get; private set; }
    }
}
