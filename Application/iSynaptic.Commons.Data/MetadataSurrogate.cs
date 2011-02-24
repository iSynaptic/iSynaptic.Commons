using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public abstract class MetadataSurrogate<TSubject> : IMetadataBindingSource, IFluentInterface
    {
        private readonly HashSet<IMetadataBinding> _Bindings = new HashSet<IMetadataBinding>();

        public void Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration, TMetadata value)
        {
            Bind((IMetadataDeclaration) declaration, value);
        }

        public ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration)
        {
            return Bind<TMetadata>((IMetadataDeclaration) declaration);
        }

        public void Bind<TMetadata>(IMetadataDeclaration declaration, TMetadata value)
        {
            Guard.NotNull(declaration, "declaration");
            _Bindings.Add(MetadataBinding.Create<TMetadata, TSubject>(this, r => r.Declaration == declaration, r => value));
        }

        public ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject> Bind<TMetadata>(IMetadataDeclaration declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentMetadataBindingBuilder<TMetadata, TSubject>(this, declaration, x => _Bindings.Add(x));
        }

        IEnumerable<IMetadataBinding> IMetadataBindingSource.GetBindingsFor<TMetadata, TBindingSubject>(IMetadataRequest<TMetadata, TBindingSubject> request)
        {
            return _Bindings;
        }
    }
}
