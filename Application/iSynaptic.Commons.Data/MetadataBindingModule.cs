using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public abstract class MetadataBindingModule
    {
        private HashSet<object> _Bindings = new HashSet<object>();

        public IEnumerable<IMetadataBinding<TMetadata>> GetBindings<TMetadata>()
        {
            return _Bindings
                .OfType<IMetadataBinding<TMetadata>>();
        }

        public void Bind<T>(MetadataDeclaration<T> declaration, T value)
        {
            _Bindings.Add(new MetadataBinding<T>(declaration, value));
        }
    }
}
