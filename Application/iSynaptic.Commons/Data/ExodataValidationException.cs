using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class ExodataValidationException<TExodata> : Exception
    {
        public ExodataValidationException(IExodataDeclaration<TExodata> declaration, TExodata invalidValue, string message)
            : base(message)
        {
            Declaration = Guard.NotNull(declaration, "declaration");
            InvalidValue = invalidValue;
        }

        public IExodataDeclaration<TExodata> Declaration { get; private set; }
        public TExodata InvalidValue { get; private set; }
    }
}
