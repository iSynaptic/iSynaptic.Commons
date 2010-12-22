using System;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class FuncExtensionsTests
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
        public void CurryOneOfOneArgument()
        {
            Func<int, int> func = i => i;
            var curried = func.Curry(6);

            Assert.AreEqual(6, curried());
        }

        [Test]
        public void CurryOneOfTwoArguments()
        {
            Func<int, int, int> func = (i1, i2) => i1 + i2;
            var curried = func.Curry(6);

            Assert.AreEqual(13, curried(7));
        }

        [Test]
        public void CurryTwoOfTwoArguments()
        {
            Func<int, int, int> func = (i1, i2) => i1 + i2;
            var curried = func.Curry(6, 7);

            Assert.AreEqual(13, curried());
        }

        [Test]
        public void CurryOneOfThreeArguments()
        {
            Func<int, int, int, int> func = (i1, i2, i3) => i1 + i2 + i3;
            var curried = func.Curry(6);

            Assert.AreEqual(21, curried(7, 8));
        }

        [Test]
        public void CurryTwoOfThreeArguments()
        {
            Func<int, int, int, int> func = (i1, i2, i3) => i1 + i2 + i3;
            var curried = func.Curry(6, 7);

            Assert.AreEqual(21, curried(8));
        }

        [Test]
        public void CurryThreeOfThreeArguments()
        {
            Func<int, int, int, int> func = (i1, i2, i3) => i1 + i2 + i3;
            var curried = func.Curry(6, 7, 8);

            Assert.AreEqual(21, curried());
        }

        [Test]
        public void CurryOneOfFourArguments()
        {
            Func<int, int, int, int, int> func = (i1, i2, i3, i4) => i1 + i2 + i3 + i4;
            var curried = func.Curry(6);

            Assert.AreEqual(30, curried(7, 8, 9));
        }

        [Test]
        public void CurryTwoOfFourArguments()
        {
            Func<int, int, int, int, int> func = (i1, i2, i3, i4) => i1 + i2 + i3 + i4;
            var curried = func.Curry(6, 7);

            Assert.AreEqual(30, curried(8, 9));
        }

        [Test]
        public void CurryThreeOfFourArguments()
        {
            Func<int, int, int, int, int> func = (i1, i2, i3, i4) => i1 + i2 + i3 + i4;
            var curried = func.Curry(6, 7, 8);

            Assert.AreEqual(30, curried(9));
        }

        [Test]
        public void CurryFourOfFourArguments()
        {
            Func<int, int, int, int, int> func = (i1, i2, i3, i4) => i1 + i2 + i3 + i4;
            var curried = func.Curry(6, 7, 8, 9);

            Assert.AreEqual(30, curried());
        }

        [Test]
        public void ToAction()
        {
            int val = 0;

            Func<int> funcZero = () => { val = 7; return 7; };
            var actionZero = funcZero.ToAction();

            actionZero();
            Assert.AreEqual(7, val);

            Func<int, int> funcOne = x => { val = x; return val; };
            var actionOne = funcOne.ToAction();

            actionOne(3);
            Assert.AreEqual(3, val);

            Func<int, int, int> funcTwo = (x, y) => { val = x * y; return val; };
            var actionTwo = funcTwo.ToAction();

            actionTwo(2, 4);
            Assert.AreEqual(8, val);

            Func<int, int, int, int> funcThree = (x, y, z) => { val = x * y * z; return val; };
            var actionThree = funcThree.ToAction();

            actionThree(2, 4, 2);
            Assert.AreEqual(16, val);

            Func<int, int, int, int, int> funcFour = (w, x, y, z) => { val = w + x + y + z; return val; };
            var actionFour = funcFour.ToAction();

            actionFour(1, 2, 3, 4);
            Assert.AreEqual(10, val);
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
