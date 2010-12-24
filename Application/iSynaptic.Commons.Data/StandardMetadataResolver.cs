using System;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class StandardMetadataResolver : MetadataResolver
    {
        public StandardMetadataResolver(params MetadataBindingModule[] modules)
        {
            if(modules != null && modules.Length > 0)
                AddMetadataBindingSource(new ModuleMetadataBindingSource(modules));

            AddMetadataBindingSource<AttributeMetadataBindingSource>();
        }
    }
}
