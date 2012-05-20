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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using NUnit.Framework;
using iSynaptic.Commons.Linq;

namespace iSynaptic.Commons
{
    [MaybeMonadContractTestFixtureAttribute(typeof(Result<,>), typeof(Result))]
    [MaybeMonadContractTestFixtureAttribute(typeof(Maybe<>), typeof(Maybe))]
    public class MaybeMonadContractTests
    {
        public MaybeMonadContractTests(Type monadType, Type extensionType)
        {
            MonadType = Guard.NotNull(monadType, "monadType");
            ValueType = typeof (string);

            ClosedMonadType = MonadType.MakeGenericType(GetTypeArguments(ValueType));
            OpenValueType = MonadType.GetGenericArguments()[0];
            
            ExtensionType = Guard.NotNull(extensionType, "extensionType");
       }

        public Type MonadType { get; private set; }
        public Type ClosedMonadType { get; private set; }

        public Type ValueType { get; private set; }
        public Type OpenValueType { get; private set; }

        public Type ExtensionType { get; private set; }

        #region Core Type Contract

        [Test]
        public void MonadType_IsNotNull()
        {
            Assert.IsNotNull(MonadType);
        }

        [Test]
        public void ExtensionType_IsNotNull()
        {
            Assert.IsNotNull(ExtensionType);
        }

        [Test]
        public void MonadType_IsGenericTypeDefinition()
        {
            Assert.IsTrue(MonadType.IsGenericTypeDefinition);
        }

        [Test]
        public void MonadType_IsAStruct()
        {
            Assert.IsTrue(MonadType.IsValueType);
        }

        [Test]
        public void MonadType_FirstTypeArgument_IsTypeOfUnderlyingValue()
        {
            // First argument of the type of the underlying Value
            var method = MonadType.GetMethodsWithParameters()
                .Where(x => x.Method.IsSpecialName && x.Method.Name == "get_Value")
                .Single(x => x.Parameters.Count == 0)
                .Method;

            Assert.IsTrue(method.ReturnType == OpenValueType);
        }

        [Test]
        public void ExtensionType_IsStaticClass()
        {
            Assert.IsTrue(ExtensionType.IsSealed);
            Assert.IsTrue(ExtensionType.IsAbstract);
            Assert.IsTrue(ExtensionType.IsClass);
        }

        [Test]
        public void ExtensionType_IsNotGenericType()
        {
            Assert.IsFalse(ExtensionType.IsGenericType);
            Assert.IsFalse(ExtensionType.IsGenericTypeDefinition);
        }

        [Test]
        public void HasPublicConstructor_ThatTakesValue()
        {
            MonadType.GetConstructorsWithParameters()
                .Single(x => x.Parameters.Count == 1 &&
                             x.Parameters[0].ParameterType == OpenValueType);
        }

        [Test]
        public void HasPublicConstructor_ThatTakesAFunctionThatYieldsTheClosedMonadType()
        {
            ClosedMonadType.GetConstructorsWithParameters()
                .Single(x => x.Parameters.Count == 1 &&
                             x.Parameters[0].ParameterType == typeof (Func<>).MakeGenericType(ClosedMonadType));
        }

        [Test]
        public void HasValue_ReturnsFalse_WhenProvidedWithNullValue()
        {
            var c = ClosedMonadType.GetConstructorsWithParameters()
                .Single(x => x.Parameters.Count == 1 &&
                             x.Parameters[0].ParameterType == ValueType)
                .Method;

            
            IMaybe maybe = c.Invoke(new object[]{null}) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.IsFalse(maybe.HasValue);
        }

        [Test]
        public void Value_ThrowsInvalidOperationException_WhenConstructedWithNullValue()
        {
            var c = ClosedMonadType.GetConstructorsWithParameters()
                .Single(x => x.Parameters.Count == 1 &&
                             x.Parameters[0].ParameterType == ValueType)
                .Method;


            IMaybe maybe = c.Invoke(new object[] { null }) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.Throws<InvalidOperationException>(() => { object value = maybe.Value; });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException_WhenConstructedWithNullComputationFunction()
        {
            var c = ClosedMonadType.GetConstructorsWithParameters()
                .Single(x => x.Parameters.Count == 1 &&
                             x.Parameters[0].ParameterType == typeof(Func<>).MakeGenericType(ClosedMonadType))
                .Method;

            var ex = Assert.Throws<TargetInvocationException>(() => c.Invoke(new object[] {null}));
            var anex = ex.InnerException as ArgumentNullException;

            Assert.IsNotNull(anex);
            Assert.AreEqual("computation", anex.ParamName);
        }

        [Test]
        public void HasValue_ReturnsFalse_WhenConstructedWithParameterlessConstructor()
        {
            IMaybe maybe = Activator.CreateInstance(ClosedMonadType) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.IsFalse(maybe.HasValue);
        }

        [Test]
        public void Value_ViaNonGenericInterface_ThrowsInvalidOperationException_WhenConstructedWithParameterlessConstructor()
        {
            IMaybe maybe = Activator.CreateInstance(ClosedMonadType) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.Throws<InvalidOperationException>(() => { object value = maybe.Value; });
        }

        [Test]
        public void Value_ViaGenericInterface_ThrowsInvalidOperationException_WhenConstructedWithParameterlessConstructor()
        {
            IMaybe<string> maybe = Activator.CreateInstance(ClosedMonadType) as IMaybe<string>;

            Assert.IsNotNull(maybe);
            Assert.Throws<InvalidOperationException>(() => { object value = maybe.Value; });
        }

        [Test]
        public void Value_ViaNonGenericInterface_ThrowsInvalidOperationException_WhenUninitializedObjectCreated()
        {
            IMaybe maybe = FormatterServices.GetSafeUninitializedObject(ClosedMonadType) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.Throws<InvalidOperationException>(() => { object value = maybe.Value; });
        }

        [Test]
        public void Value_ViaGenericInterface_ThrowsInvalidOperationException_WhenUninitializedObjectCreated()
        {
            IMaybe<string> maybe = FormatterServices.GetSafeUninitializedObject(ClosedMonadType) as IMaybe<string>;

            Assert.IsNotNull(maybe);
            Assert.Throws<InvalidOperationException>(() => { object value = maybe.Value; });
        }

        [Test]
        public void ReturnsValue_ProvidedThroughConstructor()
        {
            const string expected = "Monads rocks!";

            IMaybe maybe = Activator.CreateInstance(ClosedMonadType, expected) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(expected, maybe.Value);
        }

        [Test]
        public void ReturnsNoValue_ProvidedThroughComputation()
        {
            object computationValue = Activator.CreateInstance(ClosedMonadType);
            object computation = Expression.Lambda(typeof(Func<>).MakeGenericType(ClosedMonadType), Expression.Constant(computationValue))
                .Compile();

            IMaybe maybe = Activator.CreateInstance(ClosedMonadType, computation) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.IsFalse(maybe.HasValue);
        }

        [Test]
        public void ReturnsValue_ProvidedThroughComputation()
        {
            const string expected = "Monads rocks!";

            object computationValue = Activator.CreateInstance(ClosedMonadType, expected);
            object computation = Expression.Lambda(typeof(Func<>).MakeGenericType(ClosedMonadType), Expression.Constant(computationValue))
                .Compile();

            IMaybe maybe = Activator.CreateInstance(ClosedMonadType, computation) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(expected, maybe.Value);
        }

        [Test]
        public void ReturnsValue_IfTypeIsUnit()
        {
            IMaybe maybe = FormatterServices.GetSafeUninitializedObject(MonadType.MakeGenericType(GetTypeArguments(typeof(Unit)))) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(new Unit(), maybe.Value);
        }

        [Test]
        public void ImplementsNonGenericIMaybeInterface()
        {
            Assert.IsTrue(typeof(IMaybe).IsAssignableFrom(ClosedMonadType));
        }

        [Test]
        public void ImplementsGenericIMaybeInterface()
        {
            Assert.IsTrue(typeof(IMaybe<string>).IsAssignableFrom(ClosedMonadType));
        }
        
        [Test]
        public void ImplementsIEquatableOfValueType()
        {
            Assert.IsTrue(typeof(IEquatable<string>).IsAssignableFrom(ClosedMonadType));
        }

        [Test]
        public void ImplementsIEquatableOfMonadType()
        {
            Assert.IsTrue(typeof(IEquatable<>).MakeGenericType(ClosedMonadType).IsAssignableFrom(ClosedMonadType));
        }

        [Test]
        public void NoMethods_OnMonadType_ContainOptionalArguments()
        {
            var methods = ClosedMonadType.GetMethodsWithParameters().ToArray();

            Assert.IsTrue(methods.Length > 0);

            var outcome = methods.SelectMany(x => x.Parameters.Select(y => 
                    Outcome.FailIf(y.IsDefined(typeof(OptionalAttribute), true), String.Format("{0}.{1}.{2} is optional.", ClosedMonadType.Name, x.Method.Name, y.Name))))
                .Combine();

            Assert.IsTrue(outcome.WasSuccessful, outcome.Observations.Delimit("\r\n"));
        }

        //TODO: Refactor extension method to remove optional arguments

        //[Test]
        //public void NoMethods_OnExtensionType_ContainOptionalArguments()
        //{
        //    var methods = ExtensionType.GetMethodsWithParameters().ToArray();

        //    Assert.IsTrue(methods.Length > 0);

        //    var outcome = methods.SelectMany(x => x.Parameters.Select(y =>
        //            Outcome.FailIf(y.IsDefined(typeof(OptionalAttribute), true), String.Format("{0}.{1}.{2} is optional.", ClosedMonadType.Name, x.Method.Name, y.Name))))
        //        .Combine();

        //    Assert.IsTrue(outcome.WasSuccessful, outcome.Observations.Delimit("\r\n"));
        //}

        [Test]
        public void OverridesObjectEquals()
        {
            var equals = ClosedMonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "Equals")
                .Single(x => x.Parameters.Count == 1 && x.Parameters[0].ParameterType == typeof (object));

            Assert.IsTrue(equals.Method.DeclaringType == ClosedMonadType);
        }

        [Test]
        public void OverridesObjectGetHashCode()
        {
            var equals = ClosedMonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "GetHashCode")
                .Single(x => x.Parameters.Count == 0);

            Assert.IsTrue(equals.Method.DeclaringType == ClosedMonadType);
        }

        [Test]
        public void ImplementsEqualityOperator_WithLeftAndRightArgumentsAreMonadTypes()
        {
            Assert.IsNotNull(ClosedMonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "op_Equality")
                .Where(x => x.Parameters.Count == 2)
                .Where(x => x.Parameters[0].ParameterType == ClosedMonadType)
                .Single(x => x.Parameters[1].ParameterType == ClosedMonadType));
        }

        [Test]
        public void ImplementsInequalityOperator_WithLeftAndRightArgumentsAreMonadTypes()
        {
            Assert.IsNotNull(ClosedMonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "op_Inequality")
                .Where(x => x.Parameters.Count == 2)
                .Where(x => x.Parameters[0].ParameterType == ClosedMonadType)
                .Single(x => x.Parameters[1].ParameterType == ClosedMonadType));
        }

        [Test]
        public void ImplementsEqualityOperator_WithLeftAsMonadTypeAndRightAsValueType()
        {
            Assert.IsNotNull(ClosedMonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "op_Equality")
                .Where(x => x.Parameters.Count == 2)
                .Where(x => x.Parameters[0].ParameterType == ClosedMonadType)
                .Single(x => x.Parameters[1].ParameterType == ValueType));
        }

        [Test]
        public void ImplementsInequalityOperator_WithLeftAsMonadTypeAndRightAsValueType()
        {
            Assert.IsNotNull(ClosedMonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "op_Inequality")
                .Where(x => x.Parameters.Count == 2)
                .Where(x => x.Parameters[0].ParameterType == ClosedMonadType)
                .Single(x => x.Parameters[1].ParameterType == ValueType));
        }

        [Test]
        public void ImplementsEqualityOperator_WithLeftAsValueTypeAndRightAsMonadType()
        {
            Assert.IsNotNull(ClosedMonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "op_Equality")
                .Where(x => x.Parameters.Count == 2)
                .Where(x => x.Parameters[0].ParameterType == ValueType)
                .Single(x => x.Parameters[1].ParameterType == ClosedMonadType));
        }

        [Test]
        public void ImplementsInequalityOperator_WithLeftAsValueTypeAndRightAsMonadType()
        {
            Assert.IsNotNull(ClosedMonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "op_Inequality")
                .Where(x => x.Parameters.Count == 2)
                .Where(x => x.Parameters[0].ParameterType == ValueType)
                .Single(x => x.Parameters[1].ParameterType == ClosedMonadType));
        }

        [Test]
        public void MonadType_ImplementsExplicitConversionOperatorToValueType()
        {
            Assert.IsNotNull(ClosedMonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "op_Explicit")
                .Where(x => x.Parameters.Count == 1)
                .Where(x => x.Parameters[0].ParameterType == ClosedMonadType)
                .Single(x => x.Method.ReturnType == ValueType));
        }

        [Test]
        public void MonadType_ImplementsImplicitConversionOperatorToValueType()
        {
            Assert.IsNotNull(MonadType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "op_Implicit")
                .Where(x => x.Parameters.Count == 1)
                .Where(x => x.Parameters[0].ParameterType == MonadType.MakeGenericType(GetTypeArguments(typeof(Unit))))
                .Single(x => x.Method.ReturnType == MonadType));
        }

        #endregion

        [Test]
        public void Defer_WithFunctionThatReturnsValue() // M<T> Defer(Func<T>)
        {
            Assert.IsNotNull(ExtensionType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "Defer")
                .Where(x => x.Method.IsStatic)
                .Where(x => x.Method.IsGenericMethodDefinition)
                .Where(x => !x.GenericArguments[0].GenericParameterAttributes.Contains(GenericParameterAttributes.NotNullableValueTypeConstraint))
                .Where(x => x.Parameters.Count == 1)
                .Where(x => x.Parameters[0].ParameterType == typeof (Func<>).MakeGenericType(x.GenericArguments[0]))
                .SingleOrDefault(x => x.Method.ReturnType == MonadType.MakeGenericType(GetTypeArguments(x.GenericArguments[0]))));
        }

        [Test]
        public void Defer_WithFunctionThatReturnsNullableStructValue() // M<T> Defer(Func<T?>)
        {
            Assert.IsNotNull(ExtensionType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "Defer")
                .Where(x => x.Method.IsStatic)
                .Where(x => x.Method.IsGenericMethodDefinition)
                .Where(x => x.GenericArguments[0].GenericParameterAttributes.Contains(GenericParameterAttributes.NotNullableValueTypeConstraint))
                .Where(x => x.Parameters.Count == 1)
                .Where(x => x.Parameters[0].ParameterType == typeof(Func<>).MakeGenericType(typeof(Nullable<>).MakeGenericType(x.GenericArguments[0])))
                .SingleOrDefault(x => x.Method.ReturnType == MonadType.MakeGenericType(GetTypeArguments(x.GenericArguments[0]))));
        }

        [Test]
        public void Defer_WithFunctionThatReturnsMonadType() // M<T> Defer(Func<M<T>>)
        {
            Assert.IsNotNull(ExtensionType.GetMethodsWithParameters()
                .Where(x => x.Method.Name == "Defer")
                .Where(x => x.Method.IsStatic)
                .Where(x => x.Method.IsGenericMethodDefinition)
                .Where(x => x.Parameters.Count == 1)
                .Where(x => x.Parameters[0].ParameterType == typeof(Func<>).MakeGenericType(x.Method.ReturnType))
                .SingleOrDefault(x => x.Method.ReturnType == MonadType.MakeGenericType(x.Method.ReturnType.GetGenericArguments())));
        }

        private Type[] GetTypeArguments(Type firstTypeArgument)
        {
            Guard.NotNull(firstTypeArgument, "firstTypeArgument");

            return new[]{firstTypeArgument}.Concat(Enumerable.Repeat(typeof(Unit), MonadType.GetGenericArguments().Length - 1)).ToArray();
        }
    }

    internal class MaybeMonadContractTestFixtureAttribute : TestFixtureAttribute
    {
        public MaybeMonadContractTestFixtureAttribute(Type monadType, Type extensionType)
            : base(monadType, extensionType)
        {
            TypeArgs = new Type[0];
        }
    }
}