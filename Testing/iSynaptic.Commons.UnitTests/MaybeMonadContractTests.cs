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
using System.Reflection;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture(typeof(Maybe<>), typeof(Maybe), TypeArgs = new Type[0])]
    [TestFixture(typeof(Result<,>), typeof(Result), TypeArgs = new Type[0])]
    public class MaybeMonadContractTests
    {
        public MaybeMonadContractTests(Type monadType, Type extensionType)
        {
            MonadType = Guard.NotNull(monadType, "monadType");
            ClosedMonadType = MonadType.MakeGenericType(Enumerable.Repeat(typeof(string), MonadType.GetGenericArguments().Length).ToArray());

            ExtensionType = Guard.NotNull(extensionType, "extensionType");
       }

        public Type MonadType { get; private set; }
        public Type ClosedMonadType { get; private set; }
        public Type ExtensionType { get; private set; }

        [Test]
        public void TypesAdhearToExpectedConstraints()
        {
            Assert.IsNotNull(MonadType);
            Assert.IsNotNull(ExtensionType);

            Assert.IsTrue(MonadType.IsGenericTypeDefinition);
            Assert.IsTrue(MonadType.IsValueType);

            Assert.IsFalse(ExtensionType.IsGenericTypeDefinition);
            Assert.IsTrue(ExtensionType.IsSealed);
            Assert.IsTrue(ExtensionType.IsAbstract);
            Assert.IsTrue(ExtensionType.IsClass);
        }

        [Test]
        public void HasPublicConstructor_ThatTakesValue()
        {
            var constructors = ClosedMonadType.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                .Select(ctor => new { Ctor = ctor, Parameters = ctor.GetParameters() })
                .Where(x => x.Parameters.Length == 1 &&
                            x.Parameters[0].ParameterType == typeof(string))
                .Select(@t => @t.Ctor); 
            
            Assert.AreEqual(1, constructors.Count());
        }

        [Test]
        public void HasValue_ReturnsFalse_WhenConstructedWithParameterlessConstructor()
        {
            IMaybe maybe = Activator.CreateInstance(ClosedMonadType) as IMaybe;

            Assert.IsNotNull(maybe);
            Assert.IsFalse(maybe.HasValue);
        }

        [Test]
        public void Value_ThrowsInvalidOperationException_WhenConstructedWithParameterlessConstructor()
        {
            IMaybe maybe = Activator.CreateInstance(ClosedMonadType) as IMaybe;

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
        public void ImplementsNonGenericIMaybeInterface()
        {
            Assert.IsTrue(typeof(IMaybe).IsAssignableFrom(ClosedMonadType));
        }

        [Test]
        public void ImplementsGenericIMaybeInterface()
        {
            Assert.IsTrue(typeof(IMaybe<string>).IsAssignableFrom(ClosedMonadType));
        }
    }
}