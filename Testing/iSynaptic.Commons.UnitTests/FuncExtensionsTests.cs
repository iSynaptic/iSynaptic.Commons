using System;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public partial class FuncExtensionsTests
    {
        [Test]
        public void MakeConditional()
        {
            Func<int, int> func = null;

            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(x => x < 3); });

            func = x => x;
            Assert.Throws<ArgumentNullException>(() => { func.MakeConditional(null); });

            var simpleConditionalFunc = func.MakeConditional(x => x > 5);
            Assert.AreEqual(0, simpleConditionalFunc(1));
            Assert.AreEqual(6, simpleConditionalFunc(6));

            var withDefaultValueFunc = func.MakeConditional(x => x > 5, -1);
            Assert.AreEqual(-1, withDefaultValueFunc(1));
            Assert.AreEqual(6, withDefaultValueFunc(6));

            var withFalseFunc = func.MakeConditional(x => x > 5, x => x * 2);
            Assert.AreEqual(2, withFalseFunc(1));
            Assert.AreEqual(6, withFalseFunc(6));
        }

        [Test]
        public void ToAction()
        {
            int val = 0;

            Func<int> func = () => { val = 7; return 7; };
            var action = func.ToAction();

            action();
            Assert.AreEqual(7, val);
        }

        [Test]
        public void And_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, bool> nullFunc = null;
            Func<int, bool> notNullFunc = x => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.And(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.And(nullFunc));
        }

        [Test]
        public void And_WithValidFuncs_ComposesCorrectly()
        {
            Func<int, bool> isEven = x => x%2 == 0;
            Func<int, bool> isGreaterThanEight = x => x > 8;

            var andFunc = isEven.And(isGreaterThanEight);

            Assert.IsTrue(andFunc(10));
            Assert.IsFalse(andFunc(8));
            Assert.IsFalse(andFunc(11));
        }

        [Test]
        public void Or_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, bool> nullFunc = null;
            Func<int, bool> notNullFunc = x => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.Or(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.Or(nullFunc));
        }

        [Test]
        public void Or_WithValidFuncs_ComposesCorrectly()
        {
            Func<int, bool> isEven = x => x % 2 == 0;
            Func<int, bool> isGreaterThanEight = x => x > 8;

            var andFunc = isEven.Or(isGreaterThanEight);

            Assert.IsTrue(andFunc(10));
            Assert.IsTrue(andFunc(8));
            Assert.IsTrue(andFunc(11));
        }

        [Test]
        public void XOr_WithNullFuncs_ThrowsArgumentNullException()
        {
            Func<int, bool> nullFunc = null;
            Func<int, bool> notNullFunc = x => true;

            Assert.Throws<ArgumentNullException>(() => nullFunc.XOr(notNullFunc));
            Assert.Throws<ArgumentNullException>(() => notNullFunc.XOr(nullFunc));
        }

        [Test]
        public void XOr_WithValidFuncs_ComposesCorrectly()
        {
            Func<int, bool> isEven = x => x % 2 == 0;
            Func<int, bool> isGreaterThanEight = x => x > 8;

            var andFunc = isEven.XOr(isGreaterThanEight);

            Assert.IsFalse(andFunc(10));
            Assert.IsTrue(andFunc(8));
            Assert.IsTrue(andFunc(11));
        }
    }
}
