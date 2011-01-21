using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataBinding<TMetadata, TSubject> : IMetadataBinding<TMetadata, TSubject>
    {
        public MetadataBinding(Func<MetadataRequest<TMetadata, TSubject>, bool> predicate, Func<MetadataRequest<TMetadata, TSubject>, TMetadata> valueFactory, IMetadataBindingSource source)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            Predicate = predicate;
            ValueFactory = valueFactory;
            Source = source;
        }

        public bool Matches(MetadataRequest<TMetadata, TSubject> request)
        {
            return Predicate(request);
        }

        public TMetadata Resolve(MetadataRequest<TMetadata, TSubject> request)
        {
            return ValueFactory(request);
        }

        public Maybe<TSubject> Subject { get; set; }
        public MemberInfo Member { get; set; }
        public Func<MetadataRequest<TMetadata, TSubject>, object> ScopeFactory { get; set; }

        public Func<MetadataRequest<TMetadata, TSubject>, bool> Predicate { get; private set; }
        public Func<MetadataRequest<TMetadata, TSubject>, TMetadata> ValueFactory { get; private set; }

        public IMetadataBindingSource Source { get; private set; }
    }
}
