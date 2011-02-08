using System;

namespace iSynaptic.Commons
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly Func<IDependencyDeclaration, object> _ResolutionStrategy = null;

        public DependencyResolver(Func<IDependencyDeclaration, object> resolutionStrategy)
        {
            Guard.NotNull(resolutionStrategy, "resolutionStrategy");
            _ResolutionStrategy = resolutionStrategy;
        }

        public object Resolve(IDependencyDeclaration declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return _ResolutionStrategy(declaration);
        }
    }
}
