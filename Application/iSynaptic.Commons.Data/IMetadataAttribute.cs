using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataAttribute<TMetadata>
    {
        bool ProvidesMetadataFor<TRequestMetadata>(MetadataRequest<TRequestMetadata> request);
        TMetadata Resolve(MetadataRequest<TMetadata> request);
    }
}
