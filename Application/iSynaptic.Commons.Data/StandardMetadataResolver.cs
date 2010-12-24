using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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

        protected override IMetadataBinding<TMetadata> SelectBinding<TMetadata>(MetadataRequest<TMetadata> request, IEnumerable<IMetadataBinding<TMetadata>> candidates)
        {
            try
            {
                return candidates.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("More than one metadata binding was found. Remove duplicate bindings or apply additional conditions to existing bindings to make them unambiguous.", ex);
            }
        }
    }
}
