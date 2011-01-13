using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace iSynaptic.Commons.Data
{
    public class StandardMetadataResolver : MetadataResolver
    {
        private readonly List<MetadataBindingModule> _Modules = new List<MetadataBindingModule>();

        private class ModuleMetadataBindingSource : IMetadataBindingSource
        {
            private readonly StandardMetadataResolver _Parent;

            public ModuleMetadataBindingSource(StandardMetadataResolver parent)
            {
                Guard.NotNull(parent, "parent");
                _Parent = parent;
            }

            public IEnumerable<IMetadataBinding<TMetadata>> GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
            {
                return _Parent._Modules
                    .SelectMany(x => x.GetBindingsFor(request));
            }
        }

        public StandardMetadataResolver(params MetadataBindingModule[] modules)
        {
            if (modules != null && modules.Length > 0)
            {
                foreach (var module in modules)
                    LoadModule(module);
            }

            AddMetadataBindingSource(new ModuleMetadataBindingSource(this));
            AddMetadataBindingSource<MetadataSurrogateBindingSource>();
            AddMetadataBindingSource<AttributeMetadataBindingSource>();
        }

        protected override IMetadataBinding<TMetadata> SelectBinding<TMetadata>(MetadataRequest<TMetadata> request, IEnumerable<IMetadataBinding<TMetadata>> candidates)
        {
            var bindingList = candidates.ToList();

            if (bindingList.Count > 1)
                bindingList.RemoveAll(x => x is AttributeMetadataBinding<TMetadata>);

            return base.SelectBinding(request, bindingList);
        }

        public void LoadModule(MetadataBindingModule module)
        {
            Guard.NotNull(module, "module");
            _Modules.Add(module);
        }

        public void UnloadModule(MetadataBindingModule module)
        {
            Guard.NotNull(module, "module");
            _Modules.Remove(module);
        }
    }
}
