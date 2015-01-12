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
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using iSynaptic.Commons.Linq;

namespace iSynaptic.Commons
{
    internal delegate Outcome<string> MethodSpecification(MethodInfo candidate);

    internal static class MethodSpecificationBuilder
    {
        public static readonly MethodSpecification Any = m => Outcome.Success();

        public static MethodSpecification Named(string name)
        {
            return Any.Named(name);
        }

        public static MethodSpecification Named(this MethodSpecification @this, string name)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(m => Outcome.FailIf(m.Name != name, String.Format("Was not named '{0}'.", name)));
        }

        public static MethodSpecification IsStatic()
        {
            return Any.IsStatic();
        }

        public static MethodSpecification IsStatic(this MethodSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(m => Outcome.FailIf(!m.IsStatic, "Was not static."));
        }

        public static MethodSpecification IsPublic()
        {
            return Any.IsPublic();
        }

        public static MethodSpecification IsPublic(this MethodSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(m => Outcome.FailIf(!m.IsPublic, "Was not public."));
        }

        public static MethodSpecification IsOpenGeneric()
        {
            return Any.IsOpenGeneric();
        }

        public static MethodSpecification IsOpenGeneric(this MethodSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(m => Outcome.FailIf(!m.IsGenericMethodDefinition, "Was not open generic."));
        }

        public static MethodSpecification IsClosedGeneric()
        {
            return Any.IsClosedGeneric();
        }

        public static MethodSpecification IsClosedGeneric(this MethodSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(m => Outcome.FailIf(m.IsGenericMethodDefinition, "Was not closed generic."));
        }

        public static MethodSpecification HasGenericArguments(int count)
        {
            return Any.HasGenericArguments(count);
        }

        public static MethodSpecification HasGenericArguments(this MethodSpecification @this, int count)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(m => Outcome.FailIf(m.GetGenericArguments().Length != count, String.Format("Did not have {0} generic argument(s).", count)));
        }

        public static MethodSpecification HasParameters(int count)
        {
            return Any.HasParameters(count);
        }

        public static MethodSpecification HasParameters(this MethodSpecification @this, int count)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(m => Outcome.FailIf(m.GetParameters().Length != count, String.Format("Did not have {0} parameter(s).", count)));
        }

        public static MethodSpecification GenericArgument(Func<TypeArgumentSpecification, TypeArgumentSpecification> argumentSpecification)
        {
            return Any.GenericArgument(argumentSpecification);
        }

        public static MethodSpecification GenericArgument(Func<MethodInfo, TypeArgumentSpecification, TypeArgumentSpecification> argumentSpecification)
        {
            return Any.GenericArgument(argumentSpecification);
        }

        public static MethodSpecification GenericArgument(this MethodSpecification @this, Func<TypeArgumentSpecification, TypeArgumentSpecification> argumentSpecification)
        {
            Guard.NotNull(argumentSpecification, "argumentSpecification");
            return @this.GenericArgument((m, ga) => argumentSpecification(ga));
        }

        public static MethodSpecification GenericArgument(this MethodSpecification @this, Func<MethodInfo, TypeArgumentSpecification, TypeArgumentSpecification> argumentSpecification)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(argumentSpecification, "argumentSpecification");

            return @this.Combine(m =>
            {
                var argSpec = argumentSpecification(m, TypeArgumentSpecificationBuilder.Any);
                var outcomes = m.GetGenericArguments().Select(x => argSpec(x));
                return outcomes
                    .TryFirst(x => x.WasSuccessful)
                    .ValueOrDefault(() => outcomes.Combine());
            });
        }

        public static MethodSpecification Parameter(Func<MethodInfo, ParameterSpecification, ParameterSpecification> parameterSpecification)
        {
            return Any.Parameter(parameterSpecification);
        }

        public static MethodSpecification Parameter(this MethodSpecification @this, Func<MethodInfo, ParameterSpecification, ParameterSpecification> parameterSpecification)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(parameterSpecification, "parameterSpecification");

            return @this.Combine(m =>
            {
                var parameterSpec = parameterSpecification(m, ParameterSpecificationBuilder.Any);
                var outcomes = m.GetParameters().Select(x => parameterSpec(x));
                return outcomes
                    .TryFirst(x => x.WasSuccessful)
                    .ValueOrDefault(() => outcomes.Combine());
            });
        }

        public static MethodSpecification Returns(Func<MethodInfo, TypeSpecification, TypeSpecification> returnSpecification)
        {
            return Any.Returns(returnSpecification);
        }

        public static MethodSpecification Returns(this MethodSpecification @this, Func<MethodInfo, TypeSpecification, TypeSpecification> returnSpecification)
        {
            Guard.NotNull(@this, "this");
            Guard.NotNull(returnSpecification, "returnSpecification");

            return @this.Combine(m =>
            {
                var returnSpec = returnSpecification(m, TypeSpecificationBuilder.Any);
                return returnSpec(m.ReturnType);
            });
        }

        public static MethodSpecification IsExtensionMethod(this MethodSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(m => Outcome.FailIf(!m.GetParameters().TryElementAt(0).Select(x => x.IsDefined(typeof(ExtensionAttribute), true)).ValueOrDefault(), "Not an extension method."));
        }

        public static MethodSpecification IsNotExtensionMethod(this MethodSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return @this.Combine(m => Outcome.FailIf(m.GetParameters().TryElementAt(0).Select(x => x.IsDefined(typeof(ExtensionAttribute), true)).ValueOrDefault(), "Should not be an extension method."));
        }

        public static Func<MethodInfo, bool> ToFunc(this MethodSpecification @this)
        {
            Guard.NotNull(@this, "this");
            return m => @this(m);
        }

        public static MethodSpecification Combine(this MethodSpecification left, MethodSpecification right)
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
