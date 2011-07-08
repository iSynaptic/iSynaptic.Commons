using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public class Symbol : ISymbol { }
    public class Symbol<T> : Symbol, ISymbol<T> { }
}
