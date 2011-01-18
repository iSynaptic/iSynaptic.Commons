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
        private readonly HashSet<object> _Bindings = new HashSet<object>();

        public void Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration, TMetadata value)
        {
            Guard.NotNull(declaration, "declaration");
            _Bindings.Add(new MetadataBinding<TMetadata>(r => r.Declaration == declaration, r => value, this));
        }

        public ISubjectPredicateScopeToBinding<TMetadata> Bind<TMetadata>(MetadataDeclaration<TMetadata> declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return new FluentMetadataBindingBuilder<TMetadata>(this, r => r.Declaration == declaration, b => _Bindings.Add(b));
        }

        public IEnumerable<IMetadataBinding<TMetadata>> GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            return _Bindings
                .OfType<IMetadataBinding<TMetadata>>();
        }
    }
}
