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
using System.Linq;
using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Linq;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        [Test]
        public void Contains()
        {
            FlagsEnum f = FlagsEnum.Flag3 | FlagsEnum.Flag5;

            Assert.IsFalse(f.Contains(FlagsEnum.Flag1));
            Assert.IsFalse(f.Contains(FlagsEnum.Flag2));
            Assert.IsTrue(f.Contains(FlagsEnum.Flag3));
            Assert.IsFalse(f.Contains(FlagsEnum.Flag4));
            Assert.IsTrue(f.Contains(FlagsEnum.Flag5));

            Assert.Throws<ArgumentException>(() => f.Contains(FlagsEnum.Flag1 | FlagsEnum.Flag5));
            Assert.Throws<ArgumentException>(() => f.Contains(5));
            Assert.Throws<ArgumentException>(() => f.Contains<SpecialValue>(SpecialValue.Null));
        }

        [Test]
        public void ContainsAny()
        {
            FlagsEnum f = FlagsEnum.Flag3 | FlagsEnum.Flag5;

            Assert.IsFalse(f.ContainsAny(FlagsEnum.Flag1));
            Assert.IsFalse(f.ContainsAny(FlagsEnum.Flag2));
            Assert.IsTrue(f.ContainsAny(FlagsEnum.Flag3));
            Assert.IsFalse(f.ContainsAny(FlagsEnum.Flag4));
            Assert.IsTrue(f.ContainsAny(FlagsEnum.Flag5));

            Assert.IsFalse(f.ContainsAny(FlagsEnum.Flag1, FlagsEnum.Flag2, FlagsEnum.Flag4));
            Assert.IsTrue(f.ContainsAny(FlagsEnum.Flag1, FlagsEnum.Flag3, FlagsEnum.Flag4));
            Assert.IsTrue(f.ContainsAny(FlagsEnum.Flag1, FlagsEnum.Flag5, FlagsEnum.Flag4));

            Assert.Throws<ArgumentException>(() => f.ContainsAny(5));
            Assert.Throws<ArgumentException>(() => f.ContainsAny(SpecialValue.Null));
        }

        [Test]
        public void ContainsAll()
        {
            FlagsEnum f = FlagsEnum.Flag3 | FlagsEnum.Flag5;

            Assert.IsFalse(f.ContainsAll(FlagsEnum.Flag1));
            Assert.IsFalse(f.ContainsAll(FlagsEnum.Flag2));
            Assert.IsTrue(f.ContainsAll(FlagsEnum.Flag3));
            Assert.IsFalse(f.ContainsAll(FlagsEnum.Flag4));
            Assert.IsTrue(f.ContainsAll(FlagsEnum.Flag5));

            Assert.IsFalse(f.ContainsAll(FlagsEnum.Flag1, FlagsEnum.Flag3, FlagsEnum.Flag4));
            Assert.IsFalse(f.ContainsAll(FlagsEnum.Flag1, FlagsEnum.Flag3, FlagsEnum.Flag5));
            Assert.IsFalse(f.ContainsAll(FlagsEnum.Flag1, FlagsEnum.Flag5, FlagsEnum.Flag4));
            Assert.IsTrue(f.ContainsAll(FlagsEnum.Flag3, FlagsEnum.Flag5));

            Assert.Throws<ArgumentException>(() => f.ContainsAll(5));
            Assert.Throws<ArgumentException>(() => f.ContainsAll(SpecialValue.Null));
        }

        [Test]
        public void GetFlags()
        {
            FlagsEnum f = FlagsEnum.Flag3 | FlagsEnum.Flag5;
            Assert.IsTrue(f.GetFlags<FlagsEnum>().SequenceEqual(new FlagsEnum[] { FlagsEnum.Flag3, FlagsEnum.Flag5 }));

            Assert.Throws<ArgumentException>(() => f.GetFlags<int>().ForceEnumeration());
            Assert.Throws<ArgumentException>(() => f.GetFlags<SpecialValue>().ForceEnumeration());
        }
    }

    [Flags]
    internal enum FlagsEnum
    {
        Flag1 = 1,
        Flag2 = 2,
        Flag3 = 4,
        Flag4 = 8,
        Flag5 = 16
    }

}
