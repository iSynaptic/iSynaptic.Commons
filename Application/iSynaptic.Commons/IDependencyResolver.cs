using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public interface IDependencyResolver
    {
        object Resolve(Type dependencyType, object context);
    }
}
