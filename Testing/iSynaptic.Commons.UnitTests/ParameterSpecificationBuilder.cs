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
using System.Reflection;

namespace iSynaptic.Commons
{
    internal delegate Outcome<string> ParameterSpecification(ParameterInfo candidate);

    internal static class ParameterSpecificationBuilder
    {
        public static readonly ParameterSpecification Any = p => Outcome.Success();

        public static ParameterSpecification Named(string name)
        {
            return Any.Named(name);
        }

        public static ParameterSpecification Named(this ParameterSpecification @this, string name)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNullOrWhiteSpace(name, "name");

            return @this.Combine(x => Outcome.FailIf(x.Name != name, String.Format("Not named '{0}'.", name)));
        }

        public static ParameterSpecification Type(Func<ParameterInfo, TypeSpecification, TypeSpecification> typeSpecification)
        {
            return Any.Type(typeSpecification);
        }

        public static ParameterSpecification Type(this ParameterSpecification @this, Func<TypeSpecification, TypeSpecification> typeSpecification)
        {
            Guard.NotNull(typeSpecification, "typeSpecification");
            return @this.Type((p, ts) => typeSpecification(ts));
        }

        public static ParameterSpecification Type(this ParameterSpecification @this, Func<ParameterInfo, TypeSpecification, TypeSpecification> typeSpecification)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(typeSpecification, "typeSpecification");

            return @this.Combine(p => typeSpecification(p, TypeSpecificationBuilder.Any)(p.ParameterType));
        }

        public static ParameterSpecification AtIndex(int index)
        {
            return Any.AtIndex(index);
        }

        public static ParameterSpecification AtIndex(this ParameterSpecification @this, int index)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(p => Outcome.FailIf(p.Position != index, "Not at correct index"));
        }

        public static ParameterSpecification Combine(this ParameterSpecification left, ParameterSpecification right)
        {
            if (left == Any)
                return right;

            if (right == Any)
                return left;

            if (left == null || right == null)
                return left ?? right;

            return x =>
            {
                var leftOutcome = left(x);
                return leftOutcome.WasSuccessful
                    ? leftOutcome & right(x)
                    : leftOutcome;
            };
        }
    }
}
