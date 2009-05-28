using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace iSynaptic.Commons.UnitTests
{
    [TestFixture]
    public class CompoundExceptionTests
    {
        [Test]
        public void SimpleCreate()
        {
            CompoundException ex = new CompoundException("Simple Message");
            Assert.AreEqual("Simple Message", ex.Message);
        }
    }
}
