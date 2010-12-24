using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public interface IMetadataDeclaration<out TMetadata>
    {
        TMetadata Default { get; }
    }
}
