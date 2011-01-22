using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataBinding<TMetadata, TSubject> : IMetadataBinding<TMetadata, TSubject>
    {
        public MetadataBinding(Func<IMetadataRequest<TMetadata, TSubject>, bool> predicate, Func<IMetadataRequest<TMetadata, TSubject>, TMetadata> valueFactory, IMetadataBindingSource source)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            Predicate = predicate;
            ValueFactory = valueFactory;
            Source = source;
        }

        public bool Matches(IMetadataRequest<TMetadata, TSubject> request)
        {
            return Predicate(request);
        }

        public object GetScopeObject(IMetadataRequest<TMetadata, TSubject> request)
        {
            if (ScopeFactory != null)
                return ScopeFactory(request);

            return null;
        }

        public TMetadata Resolve(IMetadataRequest<TMetadata, TSubject> request)
        {
            return ValueFactory(request);
        }

        public bool BoundToSubjectInstance { get; set; }
        public bool BoundToMember { get; set; }

        public Func<IMetadataRequest<TMetadata, TSubject>, object> ScopeFactory { get; set; }

        public Func<IMetadataRequest<TMetadata, TSubject>, bool> Predicate { get; private set; }
        public Func<IMetadataRequest<TMetadata, TSubject>, TMetadata> ValueFactory { get; private set; }

        public IMetadataBindingSource Source { get; private set; }

        public Type SubjectType { get { return typeof (TSubject); } }
    }
}
