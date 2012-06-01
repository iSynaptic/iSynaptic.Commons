// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
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

namespace iSynaptic.Commons
{
    internal delegate Outcome<string> TypeSpecification(Type candidate);

    internal static class TypeSpecificationBuilder
    {
        public static readonly TypeSpecification Any = t => Outcome.Success();

        public static TypeSpecification IsGenericArgument()
        {
            return Any.IsGenericArgument();
        }

        public static TypeSpecification IsGenericArgument(this TypeSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(t => Outcome.FailIf(!t.IsGenericParameter, "Was not a generic argument."));
        }

        public static TypeSpecification IsEqualTo(Type expected)
        {
            return Any.IsEqualTo(expected);
        }

        public static TypeSpecification IsEqualTo(this TypeSpecification @this, Type expected)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(expected, "expected");

            return @this.Combine(t => Outcome.FailIf(t != expected, String.Format("Not the expected type: {0}", expected.Name)));
        }

        public static TypeSpecification Combine(this TypeSpecification left, TypeSpecification right)
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
