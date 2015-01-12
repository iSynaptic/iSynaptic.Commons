// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
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

using System;

namespace iSynaptic.Commons.Data
{
    public class ComparableExodataDeclaration<T> : ExodataDeclaration<T> where T : IComparable<T>
    {
        public ComparableExodataDeclaration(T minValue, T maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public ComparableExodataDeclaration(T minValue, T maxValue, T @default)
            : base(@default)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        protected override Maybe<T> EnsureValid(T value, string valueName)
        {
            if(value.CompareTo(MinValue) < 0)
                return Maybe.Throw<T>(new ExodataValidationException<T>(this, value, string.Format("The {0} value must be greater than or equal to {1}.", valueName, MinValue)));

            if(value.CompareTo(MaxValue) > 0)
                return Maybe.Throw<T>(new ExodataValidationException<T>(this, value, string.Format("The {0} value must be less than or equal to {1}.", valueName, MaxValue)));

            return base.EnsureValid(value, valueName);
        }

        public T MinValue { get; private set; }
        public T MaxValue { get; private set; }
    }
}
