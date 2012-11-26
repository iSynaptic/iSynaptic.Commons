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
using iSynaptic.Commons.Linq;

namespace iSynaptic.Commons.Reflection
{
    public static class DynamicDispatcher
    {
        private enum PassingMode
        {
            Input,
            Output,
            Reference
        }

        private abstract class FunctionInput
        {
            public FunctionInput(Type type, PassingMode passingMode)
            {
                Type = type;
                PassingMode = passingMode;
            }

            public Type Type { get; private set; }
            public PassingMode PassingMode { get; private set; }
        }

        private class Argument : FunctionInput { public Argument(Type type, PassingMode passingMode) : base(type, passingMode) { } }
        private class Parameter : FunctionInput
        {
            public Parameter(Type type, PassingMode passingMode, bool isOptional, bool isParamsArrayMember) : base(type, passingMode)
            {
                IsOptional = isOptional;
                IsParamsArrayMember = isParamsArrayMember;
            }

            public bool IsOptional { get; private set; }
            public bool IsParamsArrayMember { get; private set; }
        }

        private class ApplicableFunction
        {
            public ApplicableFunction(MethodInfo info, IEnumerable<Parameter> parameters, bool normalForm)
            {
                Info = info;
                Parameters = parameters.ToArray();
                NormalForm = normalForm;
            }

            public MethodInfo Info { get; private set; }
            public IEnumerable<Parameter> Parameters { get; private set; }
            public bool NormalForm { get; private set; }
        }

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

            var delegateMethodInfo = delegateType
                .GetMethod("Invoke");

            var delegateParameters = delegateMethodInfo
                .GetParameters();

            if(delegateParameters.Length <= 0)
                throw new ArgumentException("The provided delegate type must take in at least one parameter; the target object to dispatch to.", "TDelegate");

            if(!delegateParameters[0].ParameterType.IsAssignableFrom(targetType))
                throw new ArgumentException("The target type must be assignable to the first argument on the provided delegate type.", "targetType");

            MethodInfo[] candidateMethods = targetType
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(methodPredicate)
                .ToArray();

            var applicableMethods = SelectApplicableMethods(delegateMethodInfo, delegateParameters, candidateMethods)
                .OrderByDescending(CompareBetterFunction)
                .ToArray();

            if (applicableMethods.Length <= 0)
            {
                if (options.MissingMethodBehavior == MissingMethodBehavior.ThrowException)
                    throw new InvalidOperationException("No applicable methods could be found to dispatch to. Make sure your delegate type or method predicate is not to strict and that methods exist that can be dispatched to.");

                if (options.MissingMethodBehavior == MissingMethodBehavior.ReturnNoOpDelegate)
                    return BuildNoOpDelegate<TDelegate>(delegateType, delegateParameters);

                return default(TDelegate);
            }

            return default(TDelegate);
        }

        private static Int32 CompareBetterFunction(ApplicableFunction left, ApplicableFunction right)
        {
            return 0;
        }

        private static IEnumerable<ApplicableFunction> SelectApplicableMethods(MethodInfo delegateMethodInfo, IEnumerable<ParameterInfo> delegateParameters, IEnumerable<MethodInfo> candidateMethods)
        {
            var arguments = delegateParameters
                .Skip(1)
                .Select(CreateArgument)
                .ToArray();

            return candidateMethods
                .Select(c => TrySelectApplicableFunction(delegateMethodInfo.ReturnType, arguments, c))
                .Squash();
        }

        private static Maybe<ApplicableFunction> TrySelectApplicableFunction(Type returnType, Argument[] arguments, MethodInfo candidateMethod)
        {
            if(!returnType.IsAssignableFrom(candidateMethod.ReturnType))
                return Maybe.NoValue;

            var parameterInfos = candidateMethod.GetParameters().ToArray();

            var parameters = parameterInfos.CreateParameters(true, arguments.Length).ToArray();

            if(!IsApplicable(arguments, parameters))
            {
                var lastArgIsParamArray = parameterInfos
                    .TryLast()
                    .Select(x => x.IsDefined(typeof(ParamArrayAttribute), false))
                    .ValueOrDefault();

                if (!lastArgIsParamArray)
                    return Maybe.NoValue;

                parameters = parameterInfos.CreateParameters(false, arguments.Length).ToArray();

                return IsApplicable(arguments, parameters)
                    ? new ApplicableFunction(candidateMethod, parameters, false).ToMaybe()
                    : Maybe.NoValue;
            }

            return new ApplicableFunction(candidateMethod, parameters, true).ToMaybe();
        }

        private static bool IsApplicable(Argument[] arguments, Parameter[] parameters)
        {
            if (arguments.Length > parameters.Length)
                return false;

            return arguments.ZipAll(parameters, (a, p) =>
            {
                if (!p.HasValue)
                    return false;

                if (!a.HasValue)
                    return p.Value.IsOptional;

                var arg = a.Value;
                var param = p.Value;

                if (arg.PassingMode != param.PassingMode)
                    return false;

                if (param.PassingMode == PassingMode.Input)
                    return param.Type.IsAssignableFrom(arg.Type);

                return arg.Type == param.Type;

            }).All();
        }

        private static IEnumerable<Parameter> CreateParameters(this ParameterInfo[] parameterInfos, bool normalForm, int argumentCount)
        {
            if (normalForm)
                return parameterInfos.Select(x => CreateParameter(x, false));

            int numberOfExpandedFormParameters = Math.Max(0, argumentCount - (parameterInfos.Length - 1));
            
            return parameterInfos
                .Take(parameterInfos.Length - 1)
                .Select(x => CreateParameter(x, false))
                .Concat(Enumerable
                    .Repeat(parameterInfos[parameterInfos.Length - 1], numberOfExpandedFormParameters)
                    .Select(x => CreateParameter(x, true)));
        }

        private static Parameter CreateParameter(this ParameterInfo parameterInfo, bool isParamsArrayMember)
        {
            return CreateFunctionInput(parameterInfo, (t, pm) => new Parameter(isParamsArrayMember ? t.GetElementType() : t, pm, parameterInfo.IsOptional, isParamsArrayMember)); }
        private static Argument CreateArgument(this ParameterInfo parameterInfo) { return CreateFunctionInput(parameterInfo, (t, pm) => new Argument(t, pm)); }

        private static T CreateFunctionInput<T>(ParameterInfo parameterInfo, Func<Type, PassingMode, T> selector)
            where T : FunctionInput
        {
            var mode = PassingMode.Input;

            if(parameterInfo.IsOut)
                mode = PassingMode.Output;
            else if(parameterInfo.IsRetval)
                mode = PassingMode.Reference;

            return selector(parameterInfo.ParameterType, mode);
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
