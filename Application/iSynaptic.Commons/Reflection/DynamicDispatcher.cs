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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Reflection
{
    public static class DynamicDispatcher
    {
        public static readonly DynamicDispatcherOptions DefaultOptions =
            new DynamicDispatcherOptions(MissingMethodBehavior.ThrowException);

        public enum MissingMethodBehavior
        {
            ThrowException,
            ReturnNull,
            ReturnNoOpDelegate
        }

        public static TDelegate Build<TDelegate>(Type targetType)
        {
            return Build<TDelegate>(targetType, m => true, DefaultOptions);
        }

        public static TDelegate Build<TDelegate>(Type targetType, Func<MethodInfo, Boolean> methodPredicate)
        {
            return Build<TDelegate>(targetType, methodPredicate, DefaultOptions);
        }

        public static TDelegate Build<TDelegate>(Type targetType, DynamicDispatcherOptions options)
        {
            return Build<TDelegate>(targetType, m => true, options);
        }

        public static TDelegate Build<TDelegate>(Type targetType, Func<MethodInfo, Boolean> methodPredicate, DynamicDispatcherOptions options)
        {
            Guard.NotNull(targetType, "targetType");
            Guard.NotNull(methodPredicate, "methodPredicate");
            Guard.NotNull(options, "options");

            Type delegateType = typeof (TDelegate);
            
            if(!typeof(Delegate).IsAssignableFrom(delegateType))
                throw new ArgumentException("The provided type argument is not a delegate type.", "TDelegate");

            var delegateParameters = delegateType
                .GetMethod("Invoke")
                .GetParameters();

            if(delegateParameters.Length <= 0)
                throw new ArgumentException("The provided delegate type must take in at least one parameter; the target object to dispatch to.", "TDelegate");

            if(!delegateParameters[0].ParameterType.IsAssignableFrom(targetType))
                throw new ArgumentException("The target type must be assignable to the first argument on the provided delegate type.", "targetType");

            MethodInfo[] candidateMethods = targetType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(methodPredicate)
                .ToArray();

            if(candidateMethods.Length <= 0)
            {
                if(options.MissingMethodBehavior == MissingMethodBehavior.ThrowException)
                    throw new InvalidOperationException("No candidate methods could be found to dispatch to. Make sure your delegate type or method predicate is not to strict and that methods exist that can be dispatched to.");

                if (options.MissingMethodBehavior == MissingMethodBehavior.ReturnNull)
                    return default(TDelegate);
                
                return BuildNoOpDelegate<TDelegate>(delegateType, delegateParameters);
            }
            

            return default(TDelegate);
        }

        private static TDelegate BuildNoOpDelegate<TDelegate>(Type delegateType, IEnumerable<ParameterInfo> delegateParameters)
        {
            var methodEnd = Expression.Label();

            var parameterExpressions = BuildParameterExpressions(delegateParameters);
            return Expression.Lambda<TDelegate>(Expression.Block(typeof(void), Expression.Return(methodEnd), Expression.Label(methodEnd)), parameterExpressions).Compile();
        }

        private static ParameterExpression[] BuildParameterExpressions(IEnumerable<ParameterInfo> parameters)
        {
            return parameters
                .Select(x => Expression.Parameter(x.ParameterType, x.Name))
                .ToArray();
        }
    }

    public class DynamicDispatcherOptions
    {
        public DynamicDispatcherOptions(DynamicDispatcher.MissingMethodBehavior missingMethodBehavior)
        {
            MissingMethodBehavior = Guard.MustBeDefined(missingMethodBehavior, "missingMethodBehavior");
        }

        public DynamicDispatcher.MissingMethodBehavior MissingMethodBehavior { get; private set; }
    }
}
