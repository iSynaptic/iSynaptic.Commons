using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataBinding<TMetadata>
    {
        bool Matches(MetadataRequest<TMetadata> request);
        Func<MetadataRequest<TMetadata>, object> ScopeFactory { get; }
        TMetadata Resolve(MetadataRequest<TMetadata> request);

        IMetadataBindingSource Source { get;}
    }
}
