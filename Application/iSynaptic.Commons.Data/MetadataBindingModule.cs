using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataBindingModule : IMetadataBindingSource, IFluentInterface
    {
        private HashSet<object> _Bindings = new HashSet<object>();

        private class FluentHelper<TMetadata> : ISubjectPredicateScopeToBinding<TMetadata>
        {
            private readonly MetadataBindingModule _Parent;

            private Type _SubjectType;
            private object _Subject;
            private MemberInfo _Member;

            private Func<MetadataRequest<TMetadata>, bool> _UserPredicate;
            private Func<MetadataRequest<TMetadata>, object> _ScopeFactory;

            public FluentHelper(MetadataBindingModule parent, Func<MetadataRequest<TMetadata>, bool> userPredicate)
            {
                Guard.NotNull(parent, "parent");
                Guard.NotNull(userPredicate, "userPredicate");

                _Parent = parent;
                _UserPredicate = userPredicate;
            }

            private bool Matches(MetadataRequest<TMetadata> request)
            {
                Guard.NotNull(request, "request");

                if (_Member != null && request.Member != _Member)
                    return false;

                if (_Subject != null && request.Subject != _Subject)
                    return false;

                return _UserPredicate(request);
            }

            public void To(TMetadata value)
            {
                _Parent._Bindings.Add(new MetadataBinding<TMetadata>(Matches, x => value, _Parent, _ScopeFactory));
            }

            public void To(Func<MetadataRequest<TMetadata>, TMetadata> valueFactory)
            {
                Guard.NotNull(valueFactory, "valueFactory");
                _Parent._Bindings.Add(new MetadataBinding<TMetadata>(Matches, valueFactory, _Parent, _ScopeFactory));
            }

            public IScopeToBinding<TMetadata> When(Func<MetadataRequest<TMetadata>, bool> userPredicate)
            {
                Guard.NotNull(userPredicate, "userPredicate");
                _UserPredicate = userPredicate;
                return this;
            }

            public IPredicateScopeToBinding<TMetadata> For<TSubject>()
            {
                _SubjectType = typeof (TSubject);
                return this;
            }

            public IPredicateScopeToBinding<TMetadata> For<TSubject>(Expression<Func<TSubject, object>> member)
            {
                Guard.NotNull(member, "member");

                _SubjectType = typeof (TSubject);
                _Member = member.ExtractMemberInfoForMetadata();
                return this;
            }

            public IPredicateScopeToBinding<TMetadata> For<TSubject>(TSubject subject)
            {
                Guard.NotNull(subject, "subject");

                _SubjectType = typeof(TSubject);
                _Subject = subject;
                return this;
            }

            public IPredicateScopeToBinding<TMetadata> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
            {
                Guard.NotNull(subject, "subject");
                Guard.NotNull(member, "member");

                _SubjectType = typeof(TSubject);
                _Subject = subject;
                _Member = member.ExtractMemberInfoForMetadata();

                return this;
            }

            public IToBinding<TMetadata> InScope(object scopeObject)
            {
                Guard.NotNull(scopeObject, "scopeObject");
                _ScopeFactory = r => scopeObject;
                return this;
            }

            public IToBinding<TMetadata> InScope(Func<MetadataRequest<TMetadata>, object> scopeFactory)
            {
                Guard.NotNull(scopeFactory, "scopeFactory");
                _ScopeFactory = scopeFactory;
                return this;
            }
        }

        public void Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration, TMetadata value)
        {
            Guard.NotNull(declaration, "declaration");
            _Bindings.Add(new MetadataBinding<TMetadata>(declaration, value, this));
        }

        public ISubjectPredicateScopeToBinding<TMetadata> Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentHelper<TMetadata>(this, r => r.Declaration == declaration);
        }

        public IEnumerable<IMetadataBinding<TMetadata>> GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            return _Bindings
                .OfType<IMetadataBinding<TMetadata>>();
        }
    }
}
