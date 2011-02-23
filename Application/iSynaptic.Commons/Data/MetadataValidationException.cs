using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataValidationException : Exception
    {
        public MetadataValidationException(IMetadataDeclaration declaration, object invalidValue, string message) : base(message)
        {
            Guard.NotNull(declaration, "declaration");
            
            Declaration = declaration;
            InvalidValue = invalidValue;
        }

        public IMetadataDeclaration Declaration { get; private set; }
        public object InvalidValue { get; private set; }
    }

    public class MetadataValidationException<T> : MetadataValidationException
    {
        public MetadataValidationException(IMetadataDeclaration declaration, T invalidValue, string message) : base(declaration, invalidValue, message)
        {
        }

        public new T InvalidValue { get { return (T) base.InvalidValue; } }
    }
}
