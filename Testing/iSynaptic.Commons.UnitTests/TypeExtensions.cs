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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace iSynaptic.Commons
{
    public static class TypeExtensions
    {
        public static IEnumerable<MethodParametersPair<MethodInfo>> GetMethodsWithParameters(this Type @this)
        {
            return GetMethodsWithParameters(@this, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        }

        public static IEnumerable<MethodParametersPair<MethodInfo>> GetMethodsWithParameters(this Type @this, BindingFlags bindingFlags)
        {
            Guard.NotNull(@this, "this");
            return @this.GetMethods(bindingFlags)
                .Select(x => new MethodParametersPair<MethodInfo>(x));
        }

        public static IEnumerable<MethodParametersPair<ConstructorInfo>> GetConstructorsWithParameters(this Type @this)
        {
            return GetConstructorsWithParameters(@this, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
        }

        public static IEnumerable<MethodParametersPair<ConstructorInfo>> GetConstructorsWithParameters(this Type @this, BindingFlags bindingFlags)
        {
            Guard.NotNull(@this, "this");
            return @this.GetConstructors(bindingFlags)
                .Select(x => new MethodParametersPair<ConstructorInfo>(x));
        }
    }

    public class MethodParametersPair<T>
        where T : MethodBase
    {
        public MethodParametersPair(T method)
        {
            Method = Guard.NotNull(method, "method");
            Parameters = new ReadOnlyCollection<ParameterInfo>(method.GetParameters().ToList());
            GenericArguments = method.IsGenericMethodDefinition 
                ? new ReadOnlyCollection<Type>(method.GetGenericArguments().ToList())
                : new ReadOnlyCollection<Type>(new List<Type>());
        }

        public T Method { get; private set; }
        public ReadOnlyCollection<ParameterInfo> Parameters { get; private set; }
        public ReadOnlyCollection<Type> GenericArguments { get; private set; }
    }
}
