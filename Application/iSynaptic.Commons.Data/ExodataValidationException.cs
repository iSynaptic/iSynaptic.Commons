using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class ExodataValidationException<TExodata> : Exception
    {
        public ExodataValidationException(ISymbol<TExodata> symbol, TExodata invalidValue, string message)
            : base(message)
        {
            Symbol = Guard.NotNull(symbol, "symbol");
            InvalidValue = invalidValue;
        }

        public ISymbol<TExodata> Symbol { get; private set; }
        public TExodata InvalidValue { get; private set; }
    }
}
