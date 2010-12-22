using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons.Data
{
    public class ModuleMetadataBindingSource : IMetadataBindingSource
    {
        private MetadataBindingModule[] _Modules = null;

        public ModuleMetadataBindingSource(MetadataBindingModule[] modules)
        {
            _Modules = modules;
        }

        public IEnumerable<IMetadataBinding> GetBindingsFor(MetadataRequest request)
        {
            return _Modules
                .SelectMany(x => x.GetBindings())
                .Where(x => x.Matches(request));
        }
    }
}