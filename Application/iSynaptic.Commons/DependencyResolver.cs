using System;

namespace iSynaptic.Commons
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly Func<ISymbol, Maybe<object>> _ResolutionStrategy = null;

        public DependencyResolver(Func<ISymbol, Maybe<object>> resolutionStrategy)
        {
            Guard.NotNull(resolutionStrategy, "resolutionStrategy");
            _ResolutionStrategy = resolutionStrategy;
        }

        public Maybe<object> TryResolve(ISymbol symbol)
        {
            Guard.NotNull(symbol, "symbol");
            return _ResolutionStrategy(symbol);
        }
    }
}
