// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
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
using System.IO;
using System.Text;

using NUnit.Framework;

using iSynaptic.Commons.Text.Parsing;

namespace iSynaptic.Commons.Text.Parsing
{
    [TestFixture]
    public class ScanningTextReaderTests
    {
        [Test]
        public void InnerReaderRequired()
        {
            Assert.Throws<ArgumentNullException>(() => new ScanningTextReader(null));
        }

        [Test]
        public void ValidatePosition()
        {
            StringReader reader = new StringReader("Test");
            ScanningTextReader scanningReader = new ScanningTextReader(reader);

            Assert.AreEqual(-1, scanningReader.Position);
            
            scanningReader.Read();
            Assert.AreEqual(0, scanningReader.Position);

            scanningReader.Read();
            scanningReader.Read();
            Assert.AreEqual(2, scanningReader.Position);

            scanningReader.Read();
            Assert.AreEqual(3, scanningReader.Position);

            scanningReader.Read();
            Assert.AreEqual(-1, scanningReader.Position);

            scanningReader.Read();
            scanningReader.Read();
            Assert.AreEqual(-1, scanningReader.Position);
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
