using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class MaybeTests
    {
        [Test]
        public void AccessingValueOnNoValueThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => Console.WriteLine(Maybe<string>.NoValue.Value));
        }

        [Test]
        public void HasValueOnNoValueReturnsFalse()
        {
            Assert.IsFalse(Maybe<string>.NoValue.HasValue);
        }

        [Test]
        public void HasValueOnAValueReturnsTrue()
        {
            Assert.IsTrue(new Maybe<string>(null).HasValue);
        }

        [Test]
        public void AccessingValueOnAValueReturnsExpectedValue()
        {
            Assert.IsNull(new Maybe<string>(null).Value);
            Assert.AreEqual("Hello, World!", new Maybe<string>("Hello, World!").Value);
        }

        [Test]
        public void DefaultValueHasNoValue()
        {
            Maybe<int> val = default(Maybe<int>);
            Assert.IsFalse(val.HasValue);
        }

        [Test]
        public void DefaultValueEqualsNoValue()
        {
            Maybe<int> val = default(Maybe<int>);
            Assert.IsTrue(val == Maybe<int>.NoValue);
        }

        [Test]
        public void NullValueDoesNotEqualNoValue()
        {
            Assert.IsTrue(new Maybe<string>(null) != Maybe<string>.NoValue);
        }
    }
}
