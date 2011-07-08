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
}
