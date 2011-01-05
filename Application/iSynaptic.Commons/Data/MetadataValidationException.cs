using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public class MetadataValidationException : Exception
    {
        public MetadataValidationException(string message) : base(message)
        {
        }
    }

    public class MetadataValidationException<T> : MetadataValidationException
    {
        public MetadataValidationException(IMetadataDeclaration<T> declaration, T invalidValue, string message) : base(message)
        {
            if(declaration == null)
                throw new ArgumentNullException("declaration");

            Declaration = declaration;
            InvalidValue = invalidValue;
        }

        public IMetadataDeclaration<T> Declaration { get; private set; }
        public T InvalidValue { get; private set; }
    }
}
