using System;

namespace iSynaptic.Commons
{
    public interface IDependencyResolver
    {
        object Resolve(string key, Type dependencyType, Type requestingType);
    }
}
