using System;

namespace iSynaptic.Commons
{
    public interface IDependencyResolver
    {
        Maybe<object> TryResolve(ISymbol symbol);
    }
}
