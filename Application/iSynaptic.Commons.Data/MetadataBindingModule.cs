using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public class MetadataBindingModule : IMetadataBindingSource, IFluentInterface
    {
        private readonly HashSet<IMetadataBinding> _Bindings = new HashSet<IMetadataBinding>();

        public void Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration, TMetadata value)
        {
            Bind((IMetadataDeclaration)declaration, value);
        }

        public ISubjectPredicateScopeToBinding<TMetadata> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration)
        {
            return Bind<TMetadata>((IMetadataDeclaration)declaration);
        }

        public void Bind<TMetadata>(IMetadataDeclaration declaration, TMetadata value)
        {
            Guard.NotNull(declaration, "declaration");
            _Bindings.Add(MetadataBinding.Create<TMetadata, object>(this, r => r.Declaration == declaration, r => value));
        }

        public ISubjectPredicateScopeToBinding<TMetadata> Bind<TMetadata>(IMetadataDeclaration declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentHelper<TMetadata>(this, declaration);
        }

        public IEnumerable<IMetadataBinding> GetBindingsFor<TMetadata, TSubject>(IMetadataRequest<TSubject> request)
        {
            return _Bindings;
        }

        private class FluentHelper<TMetadata> : BaseFluentMetadataBindingBuilder<TMetadata, object>, ISubjectPredicateScopeToBinding<TMetadata>
        {
            private readonly MetadataBindingModule _Parent;
            private readonly IMetadataDeclaration _Declaration;

            public FluentHelper(MetadataBindingModule parent, IMetadataDeclaration declaration) 
                : base(parent, declaration, x => parent._Bindings.Add(x))
            {
                _Parent = parent;
                _Declaration = declaration;
            }

            public IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>()
            {
                return UseSpecificSubject<TSubject>();
            }

            public IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>(Expression<Func<TSubject, object>> member)
            {
                Guard.NotNull(member, "member");

                return UseSpecificSubject<TSubject>()
                    .For(member);
            }

            public IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>(TSubject subject)
            {
                return UseSpecificSubject<TSubject>()
                    .For(subject);
            }

            public IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
            {
                Guard.NotNull(member, "member");

                return UseSpecificSubject<TSubject>()
                    .For(subject, member);
            }

            private ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject> UseSpecificSubject<TSubject>()
            {
                return new FluentMetadataBindingBuilder<TMetadata, TSubject>(_Parent, Declaration, x => _Parent._Bindings.Add(x));
            }

            public IPredicateScopeToBinding<TMetadata, object> For(Expression<Func<object, object>> member)
            {
                Guard.NotNull(member, "member");
                Member = member.ExtractMemberInfoForMetadata();

                return this;
            }

            public IPredicateScopeToBinding<TMetadata, object> For(object subject)
            {
                Subject = subject;

                return this;
            }

            public IPredicateScopeToBinding<TMetadata, object> For(object subject, Expression<Func<object, object>> member)
            {
                Guard.NotNull(member, "member");

                Subject = subject;
                Member = member.ExtractMemberInfoForMetadata();

                return this;
            }
        }
    }
}
