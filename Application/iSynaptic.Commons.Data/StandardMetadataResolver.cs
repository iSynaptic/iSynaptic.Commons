using System;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class StandardMetadataResolver : MetadataResolver
    {
        public StandardMetadataResolver(params MetadataBindingModule[] modules)
        {
            if(modules == null)
                throw new ArgumentNullException();

            if (modules.Length <= 0)
                return;

            AddMetadataBindingSource(new ModuleMetadataBindingSource(modules));
        }
    }
}
