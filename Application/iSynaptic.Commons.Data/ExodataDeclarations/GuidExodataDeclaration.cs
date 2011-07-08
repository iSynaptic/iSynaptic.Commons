using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Data.ExodataDeclarations
{
    public class GuidExodataDeclaration : ExodataDeclaration<Guid>
    {
        public GuidExodataDeclaration()
        {
        }

        public GuidExodataDeclaration(Guid @default) : base(@default)
        {
        }

        public GuidExodataDeclaration(bool isEmptyValid)
        {
            IsEmptyValid = isEmptyValid;
        }

        public GuidExodataDeclaration(bool isEmptyValid, Guid @default) : base(@default)
        {
            IsEmptyValid = isEmptyValid;
        }

        protected override void OnValidateValue(Guid value, string valueName)
        {
            if (value == Guid.Empty && IsEmptyValid != true)
                throw new ExodataValidationException<Guid>(this, value, string.Format("The {0} value must not equal to Guid.Empty.", valueName));

            base.OnValidateValue(value, valueName);
        }

        public bool IsEmptyValid { get; private set; }
    }
}
