using System;

namespace iSynaptic.Commons
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly Func<string, Type, Type, object> _ResolutionStrategy = null;

        public DependencyResolver(Func<string, Type, Type, object> resolutionStrategy)
        {
            if (resolutionStrategy == null)
                throw new ArgumentNullException("resolutionStrategy");

            _ResolutionStrategy = resolutionStrategy;
        }

        public object Resolve(string key, Type dependencyType, Type requestingType)
        {
            return _ResolutionStrategy(key, dependencyType, requestingType);
        }
    }
}
