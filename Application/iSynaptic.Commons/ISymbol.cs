using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public interface ISymbol
    {
    }

    public interface ISymbol<in T> : ISymbol
    {
    }

    public interface INamedSymbol : ISymbol
    {
        string Name { get; }
    }

    public interface INamedSymbol<in T> : INamedSymbol, ISymbol<T>
    {
    }
}
