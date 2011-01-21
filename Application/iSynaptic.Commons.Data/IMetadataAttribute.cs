using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataAttribute<TMetadata>
    {
        bool ProvidesMetadataFor<TRequestMetadata, TSubject>(MetadataRequest<TRequestMetadata, TSubject> request);
        TMetadata Resolve<TSubject>(MetadataRequest<TMetadata, TSubject> request);
    }
}
