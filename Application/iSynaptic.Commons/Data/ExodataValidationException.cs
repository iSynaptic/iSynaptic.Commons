using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class ExodataValidationException : Exception
    {
        public ExodataValidationException(IExodataDeclaration declaration, object invalidValue, string message) : base(message)
        {
            Guard.NotNull(declaration, "declaration");
            
            Declaration = declaration;
            InvalidValue = invalidValue;
        }

        public IExodataDeclaration Declaration { get; private set; }
        public object InvalidValue { get; private set; }
    }

    public class ExodataValidationException<T> : ExodataValidationException
    {
        public ExodataValidationException(IExodataDeclaration declaration, T invalidValue, string message) : base(declaration, invalidValue, message)
        {
        }

        public new T InvalidValue { get { return (T) base.InvalidValue; } }
    }
}
