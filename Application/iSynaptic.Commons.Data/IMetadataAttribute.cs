using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataAttribute<TMetadata>
    {
        bool ProvidesMetadataFor(MetadataRequest<TMetadata> request);
        TMetadata Resolve(MetadataRequest<TMetadata> request);
    }
}
