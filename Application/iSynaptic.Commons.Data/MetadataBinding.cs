using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataBinding : IMetadataBinding
    {
        public static MetadataBinding Create<TMetadata, TSubject>(IMetadataBindingSource source, Func<IMetadataRequest<TMetadata, TSubject>, bool> predicate, Func<IMetadataRequest<TMetadata, TSubject>, TMetadata> valueFactory, Func<IMetadataRequest<TMetadata, TSubject>, object> scopeFactory = null, bool boundToSubjectInstance = false)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            return new MetadataBinding(predicate, valueFactory, source)
                       {
                           SubjectType = typeof (TSubject),
                           ScopeFactory = scopeFactory,
                           BoundToSubjectInstance = boundToSubjectInstance
                       };
        }

        private MetadataBinding(Delegate predicate, Delegate valueFactory, IMetadataBindingSource source)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            Predicate = predicate;
            ValueFactory = valueFactory;
            Source = source;
        }

        public bool Matches<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request)
        {
            var predicate = Predicate as Func<IMetadataRequest<TMetadata, TSubject>, bool>;
            return predicate != null && predicate(request);
        }

        public object GetScopeObject<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request)
        {
            var scopeFactory = ScopeFactory as Func<IMetadataRequest<TMetadata, TSubject>, object>;
            return scopeFactory != null ? scopeFactory(request) : null;
        }

        public TMetadata Resolve<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request)
        {
            return ((Func<IMetadataRequest<TMetadata, TSubject>, TMetadata>) ValueFactory)(request);
        }

        public Type SubjectType { get; private set; }
        public IMetadataBindingSource Source { get; private set; }

        public bool BoundToSubjectInstance { get; private set; }

        private Delegate ScopeFactory { get; set; }
        private Delegate Predicate { get; set; }
        private Delegate ValueFactory { get; set; }
    }
}
