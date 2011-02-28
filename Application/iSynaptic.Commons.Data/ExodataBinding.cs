using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class ExodataBinding : IExodataBinding
    {
        public static ExodataBinding Create<TExodata, TSubject>(IExodataBindingSource source, Func<IExodataRequest<TSubject>, bool> predicate, Func<IExodataRequest<TSubject>, TExodata> valueFactory, Func<IExodataRequest<TSubject>, object> scopeFactory = null, bool boundToSubjectInstance = false)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            return new ExodataBinding(predicate, valueFactory, source)
                       {
                           SubjectType = typeof (TSubject),
                           ScopeFactory = scopeFactory,
                           BoundToSubjectInstance = boundToSubjectInstance
                       };
        }

        private ExodataBinding(Delegate predicate, Delegate valueFactory, IExodataBindingSource source)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            Predicate = predicate;
            ValueFactory = valueFactory;
            Source = source;
        }

        public bool Matches<TExodata, TSubject>(IExodataRequest<TSubject> request)
        {
            var predicate = Predicate as Func<IExodataRequest<TSubject>, bool>;
            return predicate != null && predicate(request);
        }

        public object GetScopeObject<TExodata, TSubject>(IExodataRequest<TSubject> request)
        {
            var scopeFactory = ScopeFactory as Func<IExodataRequest<TSubject>, object>;
            return scopeFactory != null ? scopeFactory(request) : null;
        }

        public TExodata Resolve<TExodata, TSubject>(IExodataRequest<TSubject> request)
        {
            return ((Func<IExodataRequest<TSubject>, TExodata>) ValueFactory)(request);
        }

        public Type SubjectType { get; private set; }
        public IExodataBindingSource Source { get; private set; }

        public bool BoundToSubjectInstance { get; private set; }

        private Delegate ScopeFactory { get; set; }
        private Delegate Predicate { get; set; }
        private Delegate ValueFactory { get; set; }
    }
}
