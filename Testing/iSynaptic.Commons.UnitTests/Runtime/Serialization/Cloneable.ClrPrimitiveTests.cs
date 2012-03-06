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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Runtime.Serialization
{
    public partial class CloneableTests
    {
        [Test]
        public void CloneString()
        {
            Assert.IsTrue(Cloneable<string>.CanClone());
            Assert.IsTrue(Cloneable<string>.CanShallowClone());

            Assert.AreEqual("Testing...", Cloneable<string>.Clone("Testing..."));
            Assert.AreEqual("Testing...", Cloneable<string>.ShallowClone("Testing..."));
        }

        [Test]
        public void CloneInt16()
        {
            Assert.IsTrue(Cloneable<Int16>.CanClone());
            Assert.IsTrue(Cloneable<Int16>.CanShallowClone());
            Assert.IsTrue(Cloneable<Int16?>.CanClone());
            Assert.IsTrue(Cloneable<Int16?>.CanShallowClone());

            Assert.AreEqual((Int16)(-16), Cloneable<Int16>.Clone(-16));
            Assert.AreEqual((Int16)(-16), Cloneable<Int16>.ShallowClone(-16));

            Assert.AreEqual((Int16?)(-16), Cloneable<Int16?>.Clone(-16));
            Assert.AreEqual((Int16?)(-16), Cloneable<Int16?>.ShallowClone(-16));
            Assert.AreEqual((Int16?)(null), Cloneable<Int16?>.Clone(null));
            Assert.AreEqual((Int16?)(null), Cloneable<Int16?>.ShallowClone(null));
        }

        [Test]
        public void CloneUInt16()
        {
            Assert.IsTrue(Cloneable<UInt16>.CanClone());
            Assert.IsTrue(Cloneable<UInt16>.CanShallowClone());
            Assert.IsTrue(Cloneable<UInt16?>.CanClone());
            Assert.IsTrue(Cloneable<UInt16?>.CanShallowClone());

            Assert.AreEqual((UInt16)16, Cloneable<UInt16>.Clone(16));
            Assert.AreEqual((UInt16)16, Cloneable<UInt16>.ShallowClone(16));

            Assert.AreEqual((UInt16?)16, Cloneable<UInt16?>.Clone(16));
            Assert.AreEqual((UInt16?)16, Cloneable<UInt16?>.ShallowClone(16));
            Assert.AreEqual((UInt16?)null, Cloneable<UInt16?>.Clone(null));
            Assert.AreEqual((UInt16?)null, Cloneable<UInt16?>.ShallowClone(null));
        }

        [Test]
        public void CloneInt32()
        {
            Assert.IsTrue(Cloneable<Int32>.CanClone());
            Assert.IsTrue(Cloneable<Int32>.CanShallowClone());
            Assert.IsTrue(Cloneable<Int32?>.CanClone());
            Assert.IsTrue(Cloneable<Int32?>.CanShallowClone());

            Assert.AreEqual((Int32)(-32), Cloneable<Int32>.Clone(-32));
            Assert.AreEqual((Int32)(-32), Cloneable<Int32>.ShallowClone(-32));

            Assert.AreEqual((Int32?)(-32), Cloneable<Int32?>.Clone(-32));
            Assert.AreEqual((Int32?)(-32), Cloneable<Int32?>.ShallowClone(-32));
            Assert.AreEqual((Int32?)(null), Cloneable<Int32?>.Clone(null));
            Assert.AreEqual((Int32?)(null), Cloneable<Int32?>.ShallowClone(null));
        }

        [Test]
        public void CloneUInt32()
        {
            Assert.IsTrue(Cloneable<UInt32>.CanClone());
            Assert.IsTrue(Cloneable<UInt32>.CanShallowClone());
            Assert.IsTrue(Cloneable<UInt32?>.CanClone());
            Assert.IsTrue(Cloneable<UInt32?>.CanShallowClone());

            Assert.AreEqual((UInt32)32, Cloneable<UInt32>.Clone(32));
            Assert.AreEqual((UInt32)32, Cloneable<UInt32>.ShallowClone(32));

            Assert.AreEqual((UInt32?)32, Cloneable<UInt32?>.Clone(32));
            Assert.AreEqual((UInt32?)32, Cloneable<UInt32?>.ShallowClone(32));
            Assert.AreEqual((UInt32?)null, Cloneable<UInt32?>.Clone(null));
            Assert.AreEqual((UInt32?)null, Cloneable<UInt32?>.ShallowClone(null));
        }

        [Test]
        public void CloneInt64()
        {
            Assert.IsTrue(Cloneable<Int64>.CanClone());
            Assert.IsTrue(Cloneable<Int64>.CanShallowClone());
            Assert.IsTrue(Cloneable<Int64?>.CanClone());
            Assert.IsTrue(Cloneable<Int64?>.CanShallowClone());

            Assert.AreEqual((Int64)(-64), Cloneable<Int64>.Clone(-64));
            Assert.AreEqual((Int64)(-64), Cloneable<Int64>.ShallowClone(-64));

            Assert.AreEqual((Int64?)(-64), Cloneable<Int64?>.Clone(-64));
            Assert.AreEqual((Int64?)(-64), Cloneable<Int64?>.ShallowClone(-64));
            Assert.AreEqual((Int64?)(null), Cloneable<Int64?>.Clone(null));
            Assert.AreEqual((Int64?)(null), Cloneable<Int64?>.ShallowClone(null));
        }

        [Test]
        public void CloneUInt64()
        {
            Assert.IsTrue(Cloneable<UInt64>.CanClone());
            Assert.IsTrue(Cloneable<UInt64>.CanShallowClone());
            Assert.IsTrue(Cloneable<UInt64?>.CanClone());
            Assert.IsTrue(Cloneable<UInt64?>.CanShallowClone());

            Assert.AreEqual((UInt64)64, Cloneable<UInt64>.Clone(64));
            Assert.AreEqual((UInt64)64, Cloneable<UInt64>.ShallowClone(64));

            Assert.AreEqual((UInt64?)64, Cloneable<UInt64?>.Clone(64));
            Assert.AreEqual((UInt64?)64, Cloneable<UInt64?>.ShallowClone(64));
            Assert.AreEqual((UInt64?)null, Cloneable<UInt64?>.Clone(null));
            Assert.AreEqual((UInt64?)null, Cloneable<UInt64?>.ShallowClone(null));
        }

        [Test]
        public void CloneByte()
        {
            Assert.IsTrue(Cloneable<Byte>.CanClone());
            Assert.IsTrue(Cloneable<Byte>.CanShallowClone());
            Assert.IsTrue(Cloneable<Byte?>.CanClone());
            Assert.IsTrue(Cloneable<Byte?>.CanShallowClone());

            Assert.AreEqual((Byte)(8), Cloneable<Byte>.Clone(8));
            Assert.AreEqual((Byte)(8), Cloneable<Byte>.ShallowClone(8));

            Assert.AreEqual((Byte?)(8), Cloneable<Byte?>.Clone(8));
            Assert.AreEqual((Byte?)(8), Cloneable<Byte?>.ShallowClone(8));
            Assert.AreEqual((Byte?)(null), Cloneable<Byte?>.Clone(null));
            Assert.AreEqual((Byte?)(null), Cloneable<Byte?>.ShallowClone(null));
        }

        [Test]
        public void CloneSByte()
        {
            Assert.IsTrue(Cloneable<SByte>.CanClone());
            Assert.IsTrue(Cloneable<SByte>.CanShallowClone());
            Assert.IsTrue(Cloneable<SByte?>.CanClone());
            Assert.IsTrue(Cloneable<SByte?>.CanShallowClone());

            Assert.AreEqual((SByte)(-8), Cloneable<SByte>.Clone(-8));
            Assert.AreEqual((SByte)(-8), Cloneable<SByte>.ShallowClone(-8));

            Assert.AreEqual((SByte?)(-8), Cloneable<SByte?>.Clone(-8));
            Assert.AreEqual((SByte?)(-8), Cloneable<SByte?>.ShallowClone(-8));
            Assert.AreEqual((SByte?)(null), Cloneable<SByte?>.Clone(null));
            Assert.AreEqual((SByte?)(null), Cloneable<SByte?>.ShallowClone(null));
        }

        [Test]
        public void CloneBoolean()
        {
            Assert.IsTrue(Cloneable<Boolean>.CanClone());
            Assert.IsTrue(Cloneable<Boolean>.CanShallowClone());
            Assert.IsTrue(Cloneable<Boolean?>.CanClone());
            Assert.IsTrue(Cloneable<Boolean?>.CanShallowClone());

            Assert.AreEqual(true, Cloneable<Boolean>.Clone(true));
            Assert.AreEqual(true, Cloneable<Boolean>.ShallowClone(true));

            Assert.AreEqual(true, Cloneable<Boolean?>.Clone(true));
            Assert.AreEqual(true, Cloneable<Boolean?>.ShallowClone(true));
            Assert.AreEqual(null, Cloneable<Boolean?>.Clone(null));
            Assert.AreEqual(null, Cloneable<Boolean?>.ShallowClone(null));
        }

        [Test]
        public void CloneDouble()
        {
            Assert.IsTrue(Cloneable<Double>.CanClone());
            Assert.IsTrue(Cloneable<Double>.CanShallowClone());

            Assert.IsTrue(Cloneable<Double?>.CanClone());
            Assert.IsTrue(Cloneable<Double?>.CanShallowClone());

            Assert.AreEqual((Double)128, Cloneable<Double>.Clone(128));
            Assert.AreEqual((Double)128, Cloneable<Double>.ShallowClone(128));

            Assert.AreEqual((Double?)128, Cloneable<Double?>.Clone(128));
            Assert.AreEqual((Double?)128, Cloneable<Double?>.ShallowClone(128));
            Assert.AreEqual((Double?)null, Cloneable<Double?>.Clone(null));
            Assert.AreEqual((Double?)null, Cloneable<Double?>.ShallowClone(null));
        }

        [Test]
        public void CloneDecimal()
        {
            Assert.IsTrue(Cloneable<decimal>.CanClone());
            Assert.IsTrue(Cloneable<decimal>.CanShallowClone());

            Assert.IsTrue(Cloneable<decimal?>.CanClone());
            Assert.IsTrue(Cloneable<decimal?>.CanShallowClone());


            Assert.AreEqual((decimal)128, Cloneable<decimal>.Clone(128));
            Assert.AreEqual((decimal)128, Cloneable<decimal>.ShallowClone(128));

            Assert.AreEqual((decimal?)128, Cloneable<decimal?>.Clone(128));
            Assert.AreEqual((decimal?)128, Cloneable<decimal?>.ShallowClone(128));
            Assert.AreEqual((decimal?)null, Cloneable<decimal?>.Clone(null));
            Assert.AreEqual((decimal?)null, Cloneable<decimal?>.ShallowClone(null));
        }

        [Test]
        public void CloneChar()
        {
            Assert.IsTrue(Cloneable<Char>.CanClone());
            Assert.IsTrue(Cloneable<Char>.CanShallowClone());

            Assert.IsTrue(Cloneable<Char?>.CanClone());
            Assert.IsTrue(Cloneable<Char?>.CanShallowClone());

            Assert.AreEqual('A', Cloneable<Char>.Clone('A'));
            Assert.AreEqual('A', Cloneable<Char>.ShallowClone('A'));

            Assert.AreEqual('A', Cloneable<Char?>.Clone('A'));
            Assert.AreEqual('A', Cloneable<Char?>.ShallowClone('A'));
            Assert.AreEqual(null, Cloneable<Char?>.Clone(null));
            Assert.AreEqual(null, Cloneable<Char?>.ShallowClone(null));
        }

        [Test]
        public void CloneSingle()
        {
            Assert.IsTrue(Cloneable<Single>.CanClone());
            Assert.IsTrue(Cloneable<Single>.CanShallowClone());

            Assert.IsTrue(Cloneable<Single?>.CanClone());
            Assert.IsTrue(Cloneable<Single?>.CanShallowClone());

            Assert.AreEqual((Single)64, Cloneable<Single>.Clone(64));
            Assert.AreEqual((Single)64, Cloneable<Single>.ShallowClone(64));

            Assert.AreEqual((Single?)64, Cloneable<Single?>.Clone(64));
            Assert.AreEqual((Single?)64, Cloneable<Single?>.ShallowClone(64));
            Assert.AreEqual((Single?)null, Cloneable<Single?>.Clone(null));
            Assert.AreEqual((Single?)null, Cloneable<Single?>.ShallowClone(null));
        }

        [Test]
        public void CloneGuid()
        {
            Guid id = new Guid("6C31A07E-F633-479b-836B-9479CD7EF92F");

            Assert.IsTrue(Cloneable<Guid>.CanClone());
            Assert.IsTrue(Cloneable<Guid>.CanShallowClone());

            Assert.IsTrue(Cloneable<Guid?>.CanClone());
            Assert.IsTrue(Cloneable<Guid?>.CanShallowClone());

            Assert.AreEqual(id, Cloneable<Guid>.Clone(id));
            Assert.AreEqual(id, Cloneable<Guid>.ShallowClone(id));

            Assert.AreEqual(id, Cloneable<Guid?>.Clone(id));
            Assert.AreEqual(id, Cloneable<Guid?>.ShallowClone(id));
            Assert.AreEqual(null, Cloneable<Guid?>.Clone(null));
            Assert.AreEqual(null, Cloneable<Guid?>.ShallowClone(null));
        }

        [Test]
        public void CloneDateTime()
        {
            DateTime now = SystemClock.UtcNow;

            Assert.IsTrue(Cloneable<DateTime>.CanClone());
            Assert.IsTrue(Cloneable<DateTime>.CanShallowClone());

            Assert.IsTrue(Cloneable<DateTime?>.CanClone());
            Assert.IsTrue(Cloneable<DateTime?>.CanShallowClone());

            Assert.AreEqual(now, Cloneable<DateTime>.Clone(now));
            Assert.AreEqual(now, Cloneable<DateTime>.ShallowClone(now));

            Assert.AreEqual(now, Cloneable<DateTime?>.Clone(now));
            Assert.AreEqual(now, Cloneable<DateTime?>.ShallowClone(now));
            Assert.AreEqual(null, Cloneable<DateTime?>.Clone(null));
            Assert.AreEqual(null, Cloneable<DateTime?>.ShallowClone(null));
        }

        [Test]
        public void CloneTimeSpan()
        {
            TimeSpan source = TimeSpan.FromMinutes(5);

            Assert.IsTrue(Cloneable<TimeSpan>.CanClone());
            Assert.IsTrue(Cloneable<TimeSpan>.CanShallowClone());

            Assert.IsTrue(Cloneable<TimeSpan?>.CanClone());
            Assert.IsTrue(Cloneable<TimeSpan?>.CanShallowClone());

            Assert.AreEqual(source, Cloneable<TimeSpan>.Clone(source));
            Assert.AreEqual(source, Cloneable<TimeSpan>.ShallowClone(source));

            Assert.AreEqual(source, Cloneable<TimeSpan?>.Clone(source));
            Assert.AreEqual(source, Cloneable<TimeSpan?>.ShallowClone(source));
            Assert.AreEqual(null, Cloneable<TimeSpan?>.Clone(null));
            Assert.AreEqual(null, Cloneable<TimeSpan?>.ShallowClone(null));
        }

        [Test]
        public void CloneIntPtr()
        {
            var source = new IntPtr(42);

            Assert.IsTrue(Cloneable<IntPtr>.CanClone());
            Assert.IsTrue(Cloneable<IntPtr>.CanShallowClone());

            Assert.IsTrue(Cloneable<IntPtr?>.CanClone());
            Assert.IsTrue(Cloneable<IntPtr?>.CanShallowClone());

            Assert.AreEqual(source, Cloneable<IntPtr>.Clone(source));
            Assert.AreEqual(source, Cloneable<IntPtr>.ShallowClone(source));

            Assert.AreEqual(source, Cloneable<IntPtr?>.Clone(source));
            Assert.AreEqual(source, Cloneable<IntPtr?>.ShallowClone(source));
            Assert.AreEqual(null, Cloneable<IntPtr?>.Clone(null));
            Assert.AreEqual(null, Cloneable<IntPtr?>.ShallowClone(null));
        }

        [Test]
        public void CloneUIntPtr()
        {
            var source = new UIntPtr(42);

            Assert.IsTrue(Cloneable<UIntPtr>.CanClone());
            Assert.IsTrue(Cloneable<UIntPtr>.CanShallowClone());

            Assert.IsTrue(Cloneable<UIntPtr?>.CanClone());
            Assert.IsTrue(Cloneable<UIntPtr?>.CanShallowClone());

            Assert.AreEqual(source, Cloneable<UIntPtr>.Clone(source));
            Assert.AreEqual(source, Cloneable<UIntPtr>.ShallowClone(source));

            Assert.AreEqual(source, Cloneable<UIntPtr?>.Clone(source));
            Assert.AreEqual(source, Cloneable<UIntPtr?>.ShallowClone(source));
            Assert.AreEqual(null, Cloneable<UIntPtr?>.Clone(null));
            Assert.AreEqual(null, Cloneable<UIntPtr?>.ShallowClone(null));
        }

        [Test]
        public void CannotCloneDelegate()
        {
            Assert.IsFalse(Cloneable<Action>.CanClone());
            Assert.IsFalse(Cloneable<Action>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<Action>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<Action>.ShallowClone(null));
        }
    }
}
