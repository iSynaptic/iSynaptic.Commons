using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    public class A
    {
        public int Foo { get; set; }
    }

    public class B
    {
        public int Foo { get; set; }
        public int Bar { get; set; }
    }

    public class C
    {
        public int Bar { get; set; }
    }

    [TestFixture]
    public class UnsafeCastTests
    {
        [Test]
        public void UnsafeCast_DelegateOfSameTypeSignature()
        {
            Predicate<string> predicate = x => x.Length % 2 == 0;
            Func<string, bool> func = UnsafeCast<Predicate<string>, Func<string, bool>>.With(predicate);

            Assert.IsTrue(func("Hello!"));
        }

        [Test]
        public void UnsafeCast_ClassToClassOfSameStructure()
        {
            A source = new A {Foo = 42};
            C result = UnsafeCast<A, C>.With(source);

            Assert.AreEqual(42, result.Bar);
        }

        [Test]
        public void UnsafeCast_ClassToClassOfSupersetStructure()
        {
            A source = new A { Foo = 42 };
            B result = UnsafeCast<A, B>.With(source);

            Assert.AreEqual(42, result.Foo);
            Assert.AreEqual(0, result.Bar);
        }
    }
}
