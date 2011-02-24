using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataBindingSource
    {
        IEnumerable<IMetadataBinding> GetBindingsFor<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request);
    }
}
