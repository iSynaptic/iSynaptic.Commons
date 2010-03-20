using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public interface IDependencyResolver
    {
        object Resolve(string key, Type dependencyType, Type requestingType);
    }
}
