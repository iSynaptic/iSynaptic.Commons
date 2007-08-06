using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using MbUnit.Framework;

using iSynaptic.Commons.Text.Parsing;

namespace iSynaptic.Commons.UnitTests.Text.Parsing
{
    [TestFixture]
    public class ScanningTextReaderTests
    {
        [Test]
        [ExpectedArgumentNullException]
        public void InnerReaderRequired()
        {
            ScanningTextReader scanningReader = new ScanningTextReader(null);
        }

        [Test]
        public void ReadNormally()
        {
            StringReader reader = new StringReader("Test");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            Assert.AreEqual('T', reader.Read());
            Assert.AreEqual('e', reader.Read());
            Assert.AreEqual('s', reader.Read());
            Assert.AreEqual('t', reader.Read());
            Assert.AreEqual(-1, reader.Read());
        }

        [Test]
        public void ReadBlock()
        {
            StringReader reader = new StringReader("Test");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            char[] buffer = new char[4];
            int count = scanningReader.ReadBlock(buffer, 0, 2);
            

            Assert.AreEqual(2, count);
            Assert.AreEqual("Te\0\0", new string(buffer));

            count = -1;
            count = scanningReader.ReadBlock(buffer, 2, 2);

            Assert.AreEqual(2, count);
            Assert.AreEqual("Test", new string(buffer));
        }

        [Test]
        public void ReadLines()
        {
            StringReader reader = new StringReader("Test\r\nTest2");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            Assert.AreEqual("Test", scanningReader.ReadLine());
            Assert.AreEqual("Test2", scanningReader.ReadLine());
        }

        [Test]
        public void ReadToEnd()
        {
            StringReader reader = new StringReader("Test\r\nTest2");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            Assert.AreEqual("Test\r\nTest2", scanningReader.ReadToEnd());
        }

        [Test]
        public void LookAheadOne()
        {
            StringReader reader = new StringReader("Test");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            Assert.AreEqual('T', scanningReader.LookAhead(0));
            Assert.AreEqual('T', scanningReader.Read());
            Assert.AreEqual('e', scanningReader.Read());
            Assert.AreEqual('s', scanningReader.LookAhead(0));
            Assert.AreEqual('s', scanningReader.Read());
            Assert.AreEqual('t', scanningReader.Read());
        }

        [Test]
        public void LookAheadMany()
        {
            StringReader reader = new StringReader("Test");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            Assert.AreEqual('t', scanningReader.LookAhead(3));
            Assert.AreEqual('e', scanningReader.LookAhead(1));
            Assert.AreEqual('s', scanningReader.LookAhead(2));
            Assert.AreEqual('T', scanningReader.LookAhead(0));
            Assert.AreEqual("Test", scanningReader.ReadToEnd());
        }

        [Test]
        public void LookAheadReturnsNegativeOneOnEnd()
        {
            StringReader reader = new StringReader("Test");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            Assert.AreEqual("Test", scanningReader.ReadToEnd());
            Assert.AreEqual(-1, scanningReader.LookAhead(0));
            Assert.AreEqual(-1, scanningReader.LookAhead(5));
        }

        [Test]
        public void PeekWithoutLookahead()
        {
            StringReader reader = new StringReader("Test");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            Assert.AreEqual('T', scanningReader.Peek());
            Assert.AreEqual('T', scanningReader.Read());
            Assert.AreEqual('e', scanningReader.Peek());
        }

        [Test]
        public void PeekWithLookahead()
        {
            StringReader reader = new StringReader("Test");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            Assert.AreEqual('T', scanningReader.LookAhead(0));
            Assert.AreEqual('s', scanningReader.LookAhead(2));
            Assert.AreEqual('T', scanningReader.Peek());
            Assert.AreEqual('T', scanningReader.Read());
            Assert.AreEqual('e', scanningReader.Read());
            Assert.AreEqual('s', scanningReader.LookAhead(0));
            Assert.AreEqual('s', scanningReader.Peek());
            Assert.AreEqual("st", scanningReader.ReadToEnd());
        }
    }
}
