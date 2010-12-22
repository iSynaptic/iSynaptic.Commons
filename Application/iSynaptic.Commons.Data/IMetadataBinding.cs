using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataBinding
    {
        bool Matches(MetadataRequest request);
        Func<MetadataRequest, object> ScopeFactory { get; }

        T Resolve<T>(MetadataRequest request);
    }
}
