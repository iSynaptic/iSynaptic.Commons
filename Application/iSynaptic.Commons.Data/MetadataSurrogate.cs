using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public abstract class MetadataSurrogate<TSubject> : IMetadataBindingSource, IFluentInterface
    {
        private readonly HashSet<object> _Bindings = new HashSet<object>();

        public IPredicateScopeToBinding<TMetadata> Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration)
        {
            return StartBuildingBinding(declaration)
                .For<TSubject>();
        }

        public IPredicateScopeToBinding<TMetadata> Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration, TSubject subject)
        {
            return StartBuildingBinding(declaration)
                .For(subject);
        }

        public IPredicateScopeToBinding<TMetadata> Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration, Expression<Func<TSubject, object>> member)
        {
            return StartBuildingBinding(declaration)
                .For(member);
        }

        public IPredicateScopeToBinding<TMetadata> Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration, TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return StartBuildingBinding(declaration)
                .For(subject, member);
        }

        private ISubjectPredicateScopeToBinding<TMetadata> StartBuildingBinding<TMetadata>(MetadataDeclaration<TMetadata> declaration)
        {
            return new FluentMetadataBindingBuilder<TMetadata>(this, r => r.Declaration == declaration, b => _Bindings.Add(b));
        }

        IEnumerable<IMetadataBinding<TMetadata>> IMetadataBindingSource.GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            return _Bindings
                .OfType<IMetadataBinding<TMetadata>>();
        }
    }
}
