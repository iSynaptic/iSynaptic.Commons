using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;
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
    }
}
