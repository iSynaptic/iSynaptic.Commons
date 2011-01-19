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

        public void Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration, TMetadata value)
        {
            Guard.NotNull(declaration, "declaration");
            _Bindings.Add(new MetadataBinding<TMetadata>(r => r.Declaration == declaration, r => value, this));
        }

        public ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject> Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentMetadataBindingBuilder<TMetadata, TSubject>(this, r => r.Declaration == declaration, b => _Bindings.Add(b));
        }

        IEnumerable<IMetadataBinding<TMetadata>> IMetadataBindingSource.GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            return _Bindings
                .OfType<IMetadataBinding<TMetadata>>();
        }
    }
}
