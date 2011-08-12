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
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class IndexerTests
    {
        [Test]
        public void ReadOnly_UsesGetter()
        {
            var indexer = Indexer.ReadOnly<int, int>(x => x*2);

            Assert.AreEqual(2, indexer[1]);
            Assert.AreEqual(4, indexer[2]);
            Assert.AreEqual(6, indexer[3]);
        }

        [Test]
        public void ReadOnly_UsesKnownIndexes()
        {
            var indexer = Indexer.ReadOnly(x => x, () => new []{1,2,3,4,5});

            Assert.IsTrue(indexer.SequenceEqual(new []{1,2,3,4,5}));
        }

        [Test]
        public void ReadWrite_UsesGetter()
        {
            var indexer = Indexer.ReadWrite<int, int>(x => x * 2, (i, v) => { });

            Assert.AreEqual(2, indexer[1]);
            Assert.AreEqual(4, indexer[2]);
            Assert.AreEqual(6, indexer[3]);
        }

        [Test]
        public void ReadWrite_UsesSetter()
        {
            int result = 0;
            var indexer = Indexer.ReadWrite<int, int>(x => x, (i, v) => result = i * v);

            indexer[2] = 4;
            Assert.AreEqual(8, result);

            indexer[3] = 3;
            Assert.AreEqual(9, result);
        }

        [Test]
        public void ReadWrite_UsesKnownIndexes()
        {
            var indexer = Indexer.ReadWrite(x => x, (i, v) => { }, () => new []{1,2,3,4,5});

            Assert.IsTrue(indexer.SequenceEqual(new[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void FromKeyedReaderWriter_UsesReaderWriter()
        {
            int result = 0;
            var krw = new KeyedReaderWriter<int, int>(x => x * 2, (i, v) => { result = i * v; return true; }, null);

            var indexer = Indexer.FromKeyedReaderWriter(krw);

            Assert.AreEqual(2, indexer[1]);
            Assert.AreEqual(4, indexer[2]);
            Assert.AreEqual(6, indexer[3]);

            indexer[2] = 4;
            Assert.AreEqual(8, result);

            indexer[3] = 3;
            Assert.AreEqual(9, result);
        }

        [Test]
        public void FromKeyedReaderWriter_UsesKnownIndexes()
        {
            var krw = new KeyedReaderWriter<int, int>(x => x, (i, v) => true, () => new[]{1,2,3,4,5});
            var indexer = Indexer.FromKeyedReaderWriter(krw);

            Assert.IsTrue(indexer.SequenceEqual(new[] { 1, 2, 3, 4, 5 }));
        }
    }
}
