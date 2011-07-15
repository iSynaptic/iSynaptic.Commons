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

        protected override Maybe<string> EnsureValid(string value, string valueName)
        {
            if(value == null)
            {
                if(IsNullPermitted != true)
                    return Maybe.Exception<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must not be null.", valueName)));

                return base.EnsureValid(value, valueName);
            }

            if (value == string.Empty && IsEmptyPermitted != true)
                return Maybe.Exception<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must not be empty.", valueName)));

            if(string.IsNullOrWhiteSpace(value) && IsWhiteSpaceOnlyPermitted)
                return Maybe.Exception<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must not be only whitespace.", valueName)));

            if (valueName.Length < MinLength)
                return Maybe.Exception<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must be at least {1} characters long.", valueName, MinLength)));

            if (valueName.Length > MaxLength)
                return Maybe.Exception<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must be no more than {1} characters long.", valueName, MaxLength)));

            return base.EnsureValid(value, valueName);
        }

        public int MinLength { get; private set; }
        public int MaxLength { get; private set; }

        public bool IsNullPermitted { get; private set; }
        public bool IsEmptyPermitted { get; private set; }
        public bool IsWhiteSpaceOnlyPermitted { get; private set; }
    }
}
