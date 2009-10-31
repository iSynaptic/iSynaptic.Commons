using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.Extensions
{
    [TestFixture]
    public class PredicateExtensionsTests
    {
        [Test]
        public void And()
        {
            Predicate<int> greaterThanThree = i => i > 3;
            Predicate<int> lessThanFive = i => i < 5;

            var andPredicate = greaterThanThree.And(lessThanFive);

            Assert.IsFalse(andPredicate(3));
            Assert.IsTrue(andPredicate(4));
            Assert.IsFalse(andPredicate(5));
        }

        [Test]
        public void Or()
        {
            Predicate<int> lessThanThree = i => i < 3;
            Predicate<int> greaterThanFive = i => i > 5;

            var orPredicate = lessThanThree.Or(greaterThanFive);

            Assert.IsTrue(orPredicate(2));
            Assert.IsFalse(orPredicate(4));
            Assert.IsTrue(orPredicate(6));
        }
    }
}
