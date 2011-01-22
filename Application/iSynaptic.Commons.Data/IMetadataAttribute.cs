using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataAttribute<out TMetadata>
    {
        bool ProvidesMetadataFor<TRequestMetadata, TSubject>(IMetadataRequest<TRequestMetadata, TSubject> request);
        TMetadata Resolve<TSubject>(IMetadataRequest<TMetadata, TSubject> request);
    }
}
