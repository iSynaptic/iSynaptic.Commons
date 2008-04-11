using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace iSynaptic.Commons.UnitTests
{
    [TestFixture]
    public class CompoundExceptionTests
    {
        [Test]
        public void Serialization()
        {
            CompoundException ex = new CompoundException("Message", null);
            ex.Exceptions.Add(new ArgumentNullException("arg1"));
            ex.Exceptions.Add(new ArgumentOutOfRangeException("arg2"));

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(stream, ex);
                stream.Flush();
                stream.Position = 0;

                CompoundException serializedEx = (CompoundException)formatter.Deserialize(stream);

                Assert.IsNotNull(serializedEx);
                Assert.AreEqual(2, serializedEx.Exceptions.Count);
                Assert.IsTrue(serializedEx.Exceptions[0].GetType() == typeof(ArgumentNullException));
                Assert.IsTrue(serializedEx.Exceptions[1].GetType() == typeof(ArgumentOutOfRangeException));

                Assert.AreEqual("Message", serializedEx.Message);
            }
        }

        [Test]
        public void SimpleCreate()
        {
            CompoundException ex = new CompoundException("Simple Message");
            Assert.AreEqual("Simple Message", ex.Message);
        }
    }
}
