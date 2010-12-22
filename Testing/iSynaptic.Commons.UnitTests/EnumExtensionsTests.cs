using System;
using System.Linq;
using iSynaptic.Commons.Collections.Generic;
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

            Assert.Throws<ArgumentException>(() => { f.Contains<FlagsEnum>(FlagsEnum.Flag1 | FlagsEnum.Flag5); });
            Assert.Throws<ArgumentException>(() => { f.Contains<int>(5); });
            Assert.Throws<ArgumentException>(() => { f.Contains<SpecialValue>(SpecialValue.Null); });
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

            Assert.Throws<ArgumentException>(() => { f.ContainsAny<int>(5); });
            Assert.Throws<ArgumentException>(() => { f.ContainsAny<SpecialValue>(SpecialValue.Null); });
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

            Assert.Throws<ArgumentException>(() => { f.ContainsAll<int>(5); });
            Assert.Throws<ArgumentException>(() => { f.ContainsAll<SpecialValue>(SpecialValue.Null); });
        }

        [Test]
        public void GetFlags()
        {
            FlagsEnum f = FlagsEnum.Flag3 | FlagsEnum.Flag5;
            Assert.IsTrue(f.GetFlags<FlagsEnum>().SequenceEqual(new FlagsEnum[] { FlagsEnum.Flag3, FlagsEnum.Flag5 }));

            Assert.Throws<ArgumentException>(() => { f.GetFlags<int>().ForceEnumeration(); });
            Assert.Throws<ArgumentException>(() => { f.GetFlags<SpecialValue>().ForceEnumeration(); });
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
