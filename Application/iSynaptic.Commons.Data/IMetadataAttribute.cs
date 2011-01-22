using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataAttribute<out TMetadata>
    {
        bool ProvidesMetadataFor<TSubject>(IMetadataRequest<TSubject> request);
        TMetadata Resolve<TSubject>(IMetadataRequest<TSubject> request);
    }
}
