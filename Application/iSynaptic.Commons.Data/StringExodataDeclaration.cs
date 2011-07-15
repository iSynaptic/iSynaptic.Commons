namespace iSynaptic.Commons.Data
{
    public class StringExodataDeclaration : ExodataDeclaration<string>
    {
        public StringExodataDeclaration() : this(0, int.MaxValue, null)
        {
        }

        public StringExodataDeclaration(int minLength, int maxLength, string @default) : this(minLength, maxLength, @default, true, true, true)
        {
        }

        public StringExodataDeclaration(int minLength, int maxLength, string @default, bool isNullPermitted, bool isEmptyPermitted, bool isWhiteSpacePermitted)
            : base(@default)
        {
            MinLength = minLength;
            MaxLength = maxLength;

            IsNullPermitted = isNullPermitted;
            IsEmptyPermitted = isEmptyPermitted;
            IsWhiteSpaceOnlyPermitted = isWhiteSpacePermitted;
        }

        protected override void OnValidateValue(string value, string valueName)
        {
            if(value == null)
            {
                if(IsNullPermitted != true)
                    throw new ExodataValidationException<string>(this, value, string.Format("The {0} value must not be null.", valueName));

                return;
            }

            if (value == string.Empty && IsEmptyPermitted != true)
                throw new ExodataValidationException<string>(this, value, string.Format("The {0} value must not be empty.", valueName));

            if(string.IsNullOrWhiteSpace(value) && IsWhiteSpaceOnlyPermitted)
                throw new ExodataValidationException<string>(this, value, string.Format("The {0} value must not be only whitespace.", valueName));

            if (valueName.Length < MinLength)
                throw new ExodataValidationException<string>(this, value, string.Format("The {0} value must be at least {1} characters long.", valueName, MinLength));

            if (valueName.Length > MaxLength)
                throw new ExodataValidationException<string>(this, value, string.Format("The {0} value must be no more than {1} characters long.", valueName, MaxLength));
            
            base.OnValidateValue(value, valueName);
        }

        public int MinLength { get; private set; }
        public int MaxLength { get; private set; }

        public bool IsNullPermitted { get; private set; }
        public bool IsEmptyPermitted { get; private set; }
        public bool IsWhiteSpaceOnlyPermitted { get; private set; }
    }
}
