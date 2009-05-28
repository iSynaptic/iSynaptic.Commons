using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.UnitTests.Extensions
{
    [TestFixture]
    public class FuncExtensionsTests : BaseTestFixture
    {
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
    }
}
