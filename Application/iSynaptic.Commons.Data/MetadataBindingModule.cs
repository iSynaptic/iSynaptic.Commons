using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public abstract class MetadataBindingModule
    {
        private HashSet<IMetadataBinding> _Bindings = new HashSet<IMetadataBinding>();

        public IEnumerable<IMetadataBinding> GetBindings()
        {
            return _Bindings;
        }

        public void Bind<T>(MetadataDeclaration<T> declaration, T value)
        {
            _Bindings.Add(new MetadataBinding(declaration, value));
        }
    }
}
