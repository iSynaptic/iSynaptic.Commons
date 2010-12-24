using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataAttribute
    {
        bool ProvidesMetadataFor<TMetadata>(MetadataRequest<TMetadata> request);
        TMetadata Resolve<TMetadata>(MetadataRequest<TMetadata> request);
    }
}
