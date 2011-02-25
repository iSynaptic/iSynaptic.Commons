using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public class MetadataBindingModule : IMetadataBindingSource, IFluentMetadataBindingRoot, IFluentInterface
    {
        private readonly HashSet<IMetadataBinding> _Bindings = new HashSet<IMetadataBinding>();

        public IEnumerable<IMetadataBinding> GetBindingsFor<TMetadata, TSubject>(IMetadataRequest<TSubject> request)
        {
            return _Bindings;
        }

        public IFluentMetadataBindingSubjectPredicateScopeTo<TMetadata> Bind<TMetadata>(IMetadataDeclaration declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentMetadataBindingBuilder<TMetadata>(this, declaration, b => _Bindings.Add(b));
        }

        public IFluentMetadataBindingSubjectPredicateScopeTo<TMetadata> Bind<TMetadata>(IMetadataDeclaration<TMetadata> declaration)
        {
            return Bind<TMetadata>((IMetadataDeclaration) declaration);
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
