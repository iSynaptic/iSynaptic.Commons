using System;

namespace iSynaptic.Commons.Data
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

        protected override Maybe<Guid> EnsureValid(Guid value, string valueName)
        {
            if (value == Guid.Empty && IsEmptyValid != true)
                return Maybe.Exception<Guid>(new ExodataValidationException<Guid>(this, value, string.Format("The {0} value must not equal to Guid.Empty.", valueName)));

            return base.EnsureValid(value, valueName);
        }

        public bool IsEmptyValid { get; private set; }
    }
}
