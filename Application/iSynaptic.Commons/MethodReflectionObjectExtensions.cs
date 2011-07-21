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

using System;
using System.Linq;
using System.Reflection;

namespace iSynaptic.Commons
{
    internal static class MethodReflectionObjectExtensions
    {
        public static T GetDelegate<T>(this object target, string methodName)
        {
            Guard.NotNull(target, "target");
            Guard.NotNullOrWhiteSpace(methodName, "methodName");

            Type delegateType = typeof (T);
            Type[] parameterTypes = delegateType.GetGenericArguments();

            if (delegateType.Name.StartsWith("Func"))
            {
                parameterTypes = parameterTypes
                    .Take(parameterTypes.Length - 1)
                    .ToArray();
            }

            Type targetType = target.GetType();

            var info = Maybe.NotNull(targetType.GetMethod(methodName,
               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy,
               null,
               parameterTypes,
               null));

            return info
                .Where(x => x.IsStatic)
                .Select(x => Delegate.CreateDelegate(typeof (T), x))
                .Or(info.Select(x => Delegate.CreateDelegate(typeof (T), target, x)))
                .Cast<T>()
                .ValueOrDefault();
        }
    }
}
