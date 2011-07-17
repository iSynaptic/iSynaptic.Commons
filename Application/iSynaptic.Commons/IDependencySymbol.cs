using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public interface IDependencySymbol : INamedSymbol
    {
        Type DependencyType { get; }
    }
}
