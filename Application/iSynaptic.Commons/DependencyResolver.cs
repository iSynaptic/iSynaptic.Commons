using System;

namespace iSynaptic.Commons
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly Func<IDependencyDeclaration, Maybe<object>> _ResolutionStrategy = null;

        public DependencyResolver(Func<IDependencyDeclaration, Maybe<object>> resolutionStrategy)
        {
            Guard.NotNull(resolutionStrategy, "resolutionStrategy");
            _ResolutionStrategy = resolutionStrategy;
        }

        public Maybe<object> TryResolve(IDependencyDeclaration declaration)
        {
            Guard.NotNull(declaration, "declaration");
            return _ResolutionStrategy(declaration);
        }
    }
}
