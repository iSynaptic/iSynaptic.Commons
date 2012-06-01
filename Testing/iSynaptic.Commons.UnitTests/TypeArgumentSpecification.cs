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
using System.Reflection;

namespace iSynaptic.Commons
{
    internal delegate Outcome<string> TypeArgumentSpecification(Type candidate);

    internal static class TypeArgumentSpecificationBuilder
    {
        public static readonly TypeArgumentSpecification Any = t => Outcome.Success();

        public static TypeArgumentSpecification Type(Func<Type, TypeSpecification, TypeSpecification> typeSpecification)
        {
            return Any.Type(typeSpecification);
        }

        public static TypeArgumentSpecification Type(this TypeArgumentSpecification @this, Func<Type, TypeSpecification, TypeSpecification> typeSpecification)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(typeSpecification, "typeSpecification");

            return @this.Combine(ta => typeSpecification(ta, TypeSpecificationBuilder.Any)(ta));
        }

        public static TypeArgumentSpecification Named(string name)
        {
            return Any.Named(name);
        }

        public static TypeArgumentSpecification Named(this TypeArgumentSpecification @this, string name)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNullOrWhiteSpace(name, "name");

            return @this.Combine(x => Outcome.FailIf(x.Name != name, String.Format("Not named '{0}'.", name)));
        }

        public static TypeArgumentSpecification AtIndex(int index)
        {
            return Any.AtIndex(index);
        }

        public static TypeArgumentSpecification AtIndex(this TypeArgumentSpecification @this, int index)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(x => Outcome.FailIf(x.GenericParameterPosition != index, "Not at correct index."));
        }

        public static TypeArgumentSpecification CanBeReferenceType()
        {
            return Any.CanBeReferenceType();
        }

        public static TypeArgumentSpecification CanBeReferenceType(this TypeArgumentSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(ta => Outcome.FailIf(ta.GenericParameterAttributes.Contains(GenericParameterAttributes.NotNullableValueTypeConstraint), "Cannot be reference type."));
        }

        public static TypeArgumentSpecification CanBeValueType()
        {
            return Any.CanBeValueType();
        }

        public static TypeArgumentSpecification CanBeValueType(this TypeArgumentSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(ta => Outcome.FailIf(ta.GenericParameterAttributes.Contains(GenericParameterAttributes.ReferenceTypeConstraint), "Cannot be value type."));
        }

        public static TypeArgumentSpecification MustBeReferenceType()
        {
            return Any.MustBeReferenceType();
        }

        public static TypeArgumentSpecification MustBeReferenceType(this TypeArgumentSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(ta => Outcome.FailIf(!ta.GenericParameterAttributes.Contains(GenericParameterAttributes.ReferenceTypeConstraint), "Cannot be reference type."));
        }

        public static TypeArgumentSpecification MustBeValueType()
        {
            return Any.MustBeValueType();
        }

        public static TypeArgumentSpecification MustBeValueType(this TypeArgumentSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(ta => Outcome.FailIf(!ta.GenericParameterAttributes.Contains(GenericParameterAttributes.NotNullableValueTypeConstraint), "Cannot be value type."));
        }

        public static TypeArgumentSpecification Combine(this TypeArgumentSpecification left, TypeArgumentSpecification right)
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
