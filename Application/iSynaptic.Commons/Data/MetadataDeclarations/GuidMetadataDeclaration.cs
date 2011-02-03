using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data.MetadataDeclarations
{
    public class GuidMetadataDeclaration : MetadataDeclaration<Guid>
    {
        public GuidMetadataDeclaration()
        {
        }

        public GuidMetadataDeclaration(Guid @default) : base(@default)
        {
        }

        public GuidMetadataDeclaration(bool isEmptyValid)
        {
            IsEmptyValid = isEmptyValid;
        }

        public GuidMetadataDeclaration(bool isEmptyValid, Guid @default) : base(@default)
        {
            IsEmptyValid = isEmptyValid;
        }

        protected override void OnValidateValue(Guid value, string valueName)
        {
            if (value == Guid.Empty && IsEmptyValid != true)
                throw new MetadataValidationException<Guid>(this, value, string.Format("The {0} value must not equal to Guid.Empty.", valueName));

            base.OnValidateValue(value, valueName);
        }

        public bool IsEmptyValid { get; private set; }
    }
}
