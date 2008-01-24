using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

namespace iSynaptic.Commons.UnitTests
{
    [TestFixture]
    public class DataEventArgsTests
    {
        [Test]
        public void InheritsFromEventArgs()
        {
            Type dataEventArgsType = typeof(DataEventArgs<>);
            
            Assert.IsTrue(typeof(EventArgs).IsAssignableFrom(dataEventArgsType));
        }

        [Test]
        public void AcceptsNullData()
        {
            var args = new DataEventArgs<string>(null);
            Assert.IsNull(args.Data);
        }

        [Test]
        public void StoresData()
        {
            var args = new DataEventArgs<string>("Testing...");
            Assert.AreEqual("Testing...", args.Data);
        }
    }
}
