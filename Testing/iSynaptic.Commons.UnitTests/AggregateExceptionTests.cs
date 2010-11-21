using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class AggregateExceptionTests
    {
        [Test]
        public void SimpleCreate()
        {
            AggregateException ex = new AggregateException("Simple Message", new[] {new Exception()});

            Assert.AreEqual("Simple Message", ex.Message);
            Assert.AreEqual(1, ex.Exceptions.Count());
        }

        [Test]
        public void Constructor_WithNullExceptions_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AggregateException("Message", null));
        }

        [Test]
        public void Constructor_WithEmptyExceptions_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new AggregateException("Message", new Exception[] { }));
        }
    }
}
