using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public class Symbol : ISymbol { }
    public class Symbol<T> : Symbol, ISymbol<T> { }

    public class NamedSymbol : INamedSymbol
    {
        private readonly string _Name = null;

        public NamedSymbol()
        {
        }

        public NamedSymbol(string name)
        {
            _Name = name;
        }

        public virtual string Name { get { return _Name; } }
    }

    public class NamedSymbol<T> : NamedSymbol, INamedSymbol<T>
    {
        public NamedSymbol()
        {
        }

        public NamedSymbol(string name) : base(name)
        {
        }
    }
}
