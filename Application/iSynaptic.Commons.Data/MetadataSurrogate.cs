using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public abstract class MetadataSurrogate<TSubject> : IMetadataBindingSource, IFluentMetadataBindingRoot<TSubject>, IFluentInterface
    {
        private readonly HashSet<IMetadataBinding> _Bindings = new HashSet<IMetadataBinding>();

        IEnumerable<IMetadataBinding> IMetadataBindingSource.GetBindingsFor<TMetadata, TBindingSubject>(IMetadataRequest<TBindingSubject> request)
        {
            return _Bindings;
        }

        public IFluentMetadataBindingSpecificSubjectPredicateScopeTo<TMetadata, TSubject> Bind<TMetadata>(IMetadataDeclaration declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentMetadataBindingBuilder<TMetadata, TSubject>(this, declaration, b => _Bindings.Add(b));
        }

        public IFluentMetadataBindingSpecificSubjectPredicateScopeTo<TMetadata, TSubject> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration)
        {
            return Bind<TMetadata>((IMetadataDeclaration)declaration);
        }

        public void Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration, TMetadata value)
        {
            Bind<TMetadata>((IMetadataDeclaration)declaration)
                .To(value);
        }

        public void Bind<TMetadata>(IMetadataDeclaration declaration, TMetadata value)
        {
            Bind<TMetadata>(declaration)
                .To(value);
        }
    }
}
