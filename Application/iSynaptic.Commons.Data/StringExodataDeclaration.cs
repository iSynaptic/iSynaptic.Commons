// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace iSynaptic.Commons.Data
{
    public class StringExodataDeclaration : ExodataDeclaration<string>
    {
        public StringExodataDeclaration() : this(0, int.MaxValue, true, true)
        {
        }

        public StringExodataDeclaration(int minLength, int maxLength)
            : this(minLength, maxLength, true, true)
        {
        }

        public StringExodataDeclaration(int minLength, int maxLength, bool isEmptyPermitted, bool isWhiteSpacePermitted)
        {
            Initialize(minLength, maxLength, isEmptyPermitted, isWhiteSpacePermitted);
        }

        public StringExodataDeclaration(int minLength, int maxLength, bool isEmptyPermitted, bool isWhiteSpacePermitted, string @default)
            : base(@default)
        {
            Initialize(minLength, maxLength, isEmptyPermitted, isWhiteSpacePermitted);
        }

        private void Initialize(int minLength, int maxLength, bool isEmptyPermitted, bool isWhiteSpacePermitted)
        {
            MinLength = minLength;
            MaxLength = maxLength;

            IsEmptyPermitted = isEmptyPermitted;
            IsWhiteSpaceOnlyPermitted = isWhiteSpacePermitted;
        }

        protected override Maybe<string> EnsureValid(string value, string valueName)
        {
            if(value == null)
                return Maybe.Throw<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must not be null.", valueName)));

            if (value == string.Empty && IsEmptyPermitted != true)
                return Maybe.Throw<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must not be empty.", valueName)));

            if(string.IsNullOrWhiteSpace(value) && IsWhiteSpaceOnlyPermitted)
                return Maybe.Throw<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must not be only whitespace.", valueName)));

            if (valueName.Length < MinLength)
                return Maybe.Throw<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must be at least {1} characters long.", valueName, MinLength)));

            if (valueName.Length > MaxLength)
                return Maybe.Throw<string>(new ExodataValidationException<string>(this, value, string.Format("The {0} value must be no more than {1} characters long.", valueName, MaxLength)));

            return base.EnsureValid(value, valueName);
        }

        public int MinLength { get; private set; }
        public int MaxLength { get; private set; }

        public bool IsEmptyPermitted { get; private set; }
        public bool IsWhiteSpaceOnlyPermitted { get; private set; }
    }
}
